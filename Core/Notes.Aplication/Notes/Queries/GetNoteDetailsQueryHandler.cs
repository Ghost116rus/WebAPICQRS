using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Aplication.Common.Exceptions;
using Notes.Aplication.Interfaces;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Aplication.Notes.Queries
{
    public class GetNoteDetailsQueryHandler
                : IRequestHandler<GetNoteDetailsQuery, NoteDetailVm>
    {
        private readonly INotesDbContext _context;
        private readonly IMapper _mapper;

        public GetNoteDetailsQueryHandler(INotesDbContext notesDbContext, IMapper mapper) => (_context, _mapper) = (notesDbContext, mapper);

        public async Task<NoteDetailVm> Handle(GetNoteDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Notes
                .FirstOrDefaultAsync(note => note.Id == request.Id, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            return _mapper.Map<NoteDetailVm>(entity);
        }
    }
}
