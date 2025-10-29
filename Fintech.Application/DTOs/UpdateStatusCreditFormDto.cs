using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fintech.Application.DTOs;

public class UpdateStatusCreditFormDto
{
    [DefaultValue("OnReview")]
    [AllowedValues("Draft", "Pending", "OnReview", "Approved", "Rejected")]
    public string Status { get; set; } = "OnReview";
}