using System.ComponentModel.DataAnnotations;

namespace Web_Digital_Potrfolio.ViewModels;

public class PortfolioViewModel
{
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(125, ErrorMessage = "Большое название")]
    [Display(Name = "Название портфолио")]
    public string? Title { get; set; }
    
    [Display(Name = "Текст")]
    public string? Text { get; set; }
    
    [Required(ErrorMessage = "Ссылка обязательно")]
    [Display(Name = "Ссылка на GitHub")]
    public string? GitHubUrl { get; set; }
    
    public long UserId { get; set; }
}