using CSharpFunctionalExtensions;
using RaritetBooks.Domain.Common;
using RaritetBooks.Domain.ValueObjects;
using Entity = RaritetBooks.Domain.Common.Entity;

namespace RaritetBooks.Domain.Entities;

public class SellerRequest : Entity
{
    public FullName FullName { get; private set; } = null!;
    public MobilePhone PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime? RegistrationDate { get; private set; }
    public SellerRequestStatus Status { get; private set; } = null!;

    public SellerRequest()
    {
    }
    
    public SellerRequest(
        FullName fullName,
        MobilePhone phoneNumber,
        Email email,
        string description)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Email = email;
        Description = description;
        RegistrationDate = DateTime.UtcNow;
        Status = SellerRequestStatus.Consideration;
    }
    
    public Result<bool, Error> Approve()
    {
        if (Status == SellerRequestStatus.Approved)
            return ErrorList.SellerRequest.AlreadyApproved();
            
        Status = SellerRequestStatus.Approved;
        
        return true;
    }

    public Result<bool, Error> Decline()
    {
        if (Status == SellerRequestStatus.Declined)
            return ErrorList.SellerRequest.AlreadyDeclined();

        Status = SellerRequestStatus.Declined;

        return true;
    }
}