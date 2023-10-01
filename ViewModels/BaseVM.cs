namespace ViewModels
{
    public class BaseVM
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Status { get; set; }
    }
}
