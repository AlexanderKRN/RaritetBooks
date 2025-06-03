using FluentAssertions;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.UnitTests;

public class SellersTests
{
    [Theory]
    [MemberData(nameof(UserSellerCreateWithValidData))]
    public void UserSeller_Create_WithValidData(
        Guid userId,
        FullName fullName,
        string description,
        MobilePhone mobilePhone,
        List<SocialContact> contacts)
    {
        //arrange

        //act
        var result = UserSeller.Create(
            userId,
            fullName,
            description,
            mobilePhone,
            contacts);

        //assert
        result.IsSuccess.Should().Be(true);
        result.Value.Should().NotBe(null);
    }

    [Theory]
    [MemberData(nameof(UserSellerCreateWithInValidData))]
    public void UserSeller_Create_WithInValidData(
       Guid userId,
       FullName fullName,
       string description,
       MobilePhone mobilePhone,
       List<SocialContact> contacts)
    {
        //arrange

        //act
        var result = UserSeller.Create(
            userId,
            fullName,
            description,
            mobilePhone,
            contacts);

        //assert
        result.IsSuccess.Should().Be(false);
        result.Invoking(x => x.Value).Should()
            .Throw<CSharpFunctionalExtensions.ResultFailureException>();
    }

    public static IEnumerable<object[]> UserSellerCreateWithValidData =>
        new List<object[]>
        {
            new object[]
            {
                Guid.NewGuid(),
                FullName.Create("Aaa", "Bbb", "Ccc").Value,
                "Lorem",
                MobilePhone.Create("+79009009090").Value,
                new List<SocialContact>()
            },
            new object[]
            {
                Guid.NewGuid(),
                FullName.Create("Aaa", "Bbb", null).Value,
                "Lorem",
                MobilePhone.Create("+79009009090").Value,
                new List<SocialContact>()
            },
        };

    public static IEnumerable<object[]> UserSellerCreateWithInValidData =>
        new List<object[]>
        {
            new object[]
            {
                Guid.Empty,
                FullName.Create("Aaa", "Bbb", "Ccc").Value,
                "Lorem",
                MobilePhone.Create("+79009009090").Value,
                new List<SocialContact>()
            },
            new object[]
            {
                Guid.NewGuid(),
                FullName.Create("Aaa", "Bbb", null).Value,
                string.Empty,
                MobilePhone.Create("+79009009090").Value,
                new List<SocialContact>()
            },
        };
}