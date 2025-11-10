using NoteApi.Dto;

namespace NoteApi.Services
{
    public interface INoteService
    {
        public Task<BaseResponse<NoteListResponse<Note[]?>>> GetNoteList(NoteQueryParam query);
        public Task<BaseResponse<Note?>> GetNoteById(int id);
        public Task<BaseResponse<Note>> CreateNote(Note note);
        public Task<BaseResponse<Note?>> UpdateNote(int id, Note note);
        public Task<BaseResponse<Note?>> DeleteNote(int id);
    }
}
