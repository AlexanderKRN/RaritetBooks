using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using RaritetBooks.Application.Features.Sellers;
using RaritetBooks.Application.Features.Sellers.UploadPhoto;
using RaritetBooks.Application.Providers;
using RaritetBooks.Domain.Entities;
using RaritetBooks.Domain.ValueObjects;

namespace RaritetBooks.UnitTests;

public class UploadSellerPhotoTests
{
    private readonly Mock<IMinioProvider> _minioProviderMock = new();
    private readonly Mock<ISellerRepository> _sellerRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IFormFile> _formFileMock = new();

    public UploadSellerPhotoTests()
    {
        _formFileMock.Setup(x => x.FileName).Returns("file.png");
        _formFileMock.Setup(x => x.ContentType).Returns("file.png");
    }

    [Fact]
    public async Task UploadSellerPhotoHandler_Handler_WithValidData()
    {
        //arrange
        var ct = new CancellationToken();
        var context = new DefaultHttpContext();
        var sellerId = Guid.NewGuid();

        var request = new UploadSellerPhotoRequest(
            sellerId,
            _formFileMock.Object);

        var seller = UserSeller.Create(
            sellerId,
            FullName.Create("Aaa", "Bbb", "Ccc").Value,
            "Lorem",
            MobilePhone.Create("+79009009090").Value,
            new List<SocialContact>());

        _minioProviderMock.Setup(x => x.UploadPhoto(
            "bucketName",
            _formFileMock.Object.OpenReadStream(),
            "path",
            ct)).ReturnsAsync("path");

        _sellerRepositoryMock.Setup(x => x.GetById(sellerId, ct))
            .ReturnsAsync(seller);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(ct))
            .ReturnsAsync(1);

        var sut = new UploadSellerPhotoHandler(
            _minioProviderMock.Object,
            _sellerRepositoryMock.Object,
            _unitOfWorkMock.Object);

        //act
        var result = await sut.Handle(request, context, ct);

        //assert
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(ct), Times.Once);
        result.IsSuccess.Should().Be(true);
        result.Value.Should().BeOfType<string>();
        result.Value.Should().NotBeEmpty();
    }
}
