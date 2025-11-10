using NoteApi.Dto;

namespace NoteApi.Services
{
    public class NoteService(NoteRepository noteRepository) : INoteService
    {
        public async Task<BaseResponse<NoteListResponse<Note[]?>>> GetNoteList(NoteQueryParam query)
        {
            try
            {
                var res = await noteRepository.GetNoteList(query);

                if (res != null)
                {
                    return new BaseResponse<NoteListResponse<Note[]?>>
                    {
                        data = new NoteListResponse<Note[]?>
                        {
                            Data = res.Data,
                            Total = res.Total,
                            Limit = res.Limit,
                            Offset = res.Offset
                        },
                        success = true,
                        message = "success"
                    };
                }
                else
                {
                    return new BaseResponse<NoteListResponse<Note[]?>>
                    {
                        data = null,
                        success = false,
                        message = "Failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<Note?>> GetNoteById(int id)
        {
            try
            {
                var res = await noteRepository.GetNoteById(id);

                if (res != null)
                {
                    return new BaseResponse<Note?>
                    {
                        data = res,
                        success = true,
                        message = "success"
                    };
                }
                else
                {
                    return new BaseResponse<Note?>
                    {
                        data = null,
                        success = false,
                        message = "failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<Note>> CreateNote(Note note)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(note.Title))
                {
                    return new BaseResponse<Note>
                    {
                        data = null,
                        success = false,
                        message = "Title is required"
                    };
                }

                var res = await noteRepository.CreateNote(note);

                if (res != null)
                {
                    return new BaseResponse<Note>
                    {
                        data = res,
                        success = true,
                        message = "success"
                    };
                }
                else
                {
                    return new BaseResponse<Note>
                    {
                        data = null,
                        success = false,
                        message = "failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<Note?>> UpdateNote(int id, Note note)
        {
            try
            {
                var res = await noteRepository.UpdateNote(id, note);

                if (res != null)
                {
                    return new BaseResponse<Note?>
                    {
                        data = res,
                        success = true,
                        message = "success"
                    };
                }
                else
                {
                    return new BaseResponse<Note?>
                    {
                        data = null,
                        success = false,
                        message = "failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<Note?>> DeleteNote(int id)
        {
           try
            {
                var res = await noteRepository.DeleteNote(id);

                if (res)
                {
                    return new BaseResponse<Note?>
                    {
                        data = null,
                        success = true,
                        message = "success"
                    };
                }
                else
                {
                    return new BaseResponse<Note?>
                    {
                        data = null,
                        success = false,
                        message = "failed"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
