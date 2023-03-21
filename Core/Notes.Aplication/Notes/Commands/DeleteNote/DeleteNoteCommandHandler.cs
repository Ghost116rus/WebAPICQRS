using MediatR;
using Notes.Aplication.Common.Exceptions;
using Notes.Aplication.Interfaces;
using Notes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notes.Aplication.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler
        :IRequestHandler<DeleteNoteCommand>
    {
        private readonly INotesDbContext _context;

        public DeleteNoteCommandHandler(INotesDbContext notesDbContext) => _context = notesDbContext;

        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Notes
                .FindAsync(new object[] { request.Id }, cancellationToken);

            if (entity == null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }

            _context.Notes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
