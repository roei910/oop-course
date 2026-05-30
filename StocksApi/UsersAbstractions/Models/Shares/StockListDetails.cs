using System.ComponentModel.DataAnnotations;

namespace UsersAbstractions.Models.Shares
{
    public class StockListDetails
	{
        [Required]
        public required string UserEmail { get; set; }
        [Required]
        [StringLength(100)]
        public required string ListName { get; set; }
	}
}
