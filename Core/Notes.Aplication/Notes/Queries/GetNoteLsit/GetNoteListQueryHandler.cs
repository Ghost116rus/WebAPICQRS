using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Notes.Aplication.Interfaces;
using Notes.Aplication.Notes.Queries.GetNoteList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Aplication.Notes.Queries.GetNoteLsit
{
    public class GetNoteListQueryHandler
        : IRequestHandler<GetNoteListQuery, NoteListVM>
    {
        private readonly INotesDbContext _context;
        private readonly IMapper _mapper;

        public GetNoteListQueryHandler(INotesDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<NoteListVM> Handle(GetNoteListQuery request,
            CancellationToken cancellationToken)
        {
            var notesQuery = await _context.Notes
                .Where(note => note.UserId == request.UserId)
                .ProjectTo<NoteLookupDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new NoteListVM() { LookupList = notesQuery };
        }
    }
}
