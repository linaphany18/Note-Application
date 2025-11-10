namespace NoteApi.Dto
{
    public class NoteQueryParam
    {
        public string? Search { get; set; }
        public string? OrderBy { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? Limit { get; set; }  // page size
        public int? Offset { get; set; } // number of rows to skip
    }
}
