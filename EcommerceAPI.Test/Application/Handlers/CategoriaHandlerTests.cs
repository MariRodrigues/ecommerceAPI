﻿using AutoMapper;
using EcommerceAPI.Application.Commands.Categorias;
using EcommerceAPI.Application.Handlers.Categorias;
using EcommerceAPI.Domain.Categorias;
using EcommerceAPI.Domain.Repository;
using EcommerceAPI.Test.Application.Handlers.CategoriaCommandFactory;
using EcommerceAPI.Test.Domain.Entities.Factory;
using Moq;

using System.Threading;
using Xunit;

namespace EcommerceAPI.Test.Application.Handlers
{
    public class CategoriaHandlerTests
    {
        private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly Mock<ISubcategoriaRepository> _subcategoriaRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;

        public CategoriaHandlerTests()
        {
            _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _subcategoriaRepositoryMock = new Mock<ISubcategoriaRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        private CategoriaHandler CreateCategoriaHandler()
        {
            return new CategoriaHandler(
                _categoriaRepositoryMock.Object,
                _mapperMock.Object,
                _produtoRepositoryMock.Object,
                _subcategoriaRepositoryMock.Object);
        }

        [Fact(DisplayName = "Deve ser possível cadastrar uma categoria")]
        public async void Deve_Criar_Categoria_Valido()
        {
            // Arrange
            var command = CreateCategoriaCommandFactory.CreateCategoriaCommand();
            var categoria = CategoriaFactory.Create();

            _mapperMock.Setup(x => x.Map<Categoria>(It.IsAny<CreateCategoriaCommand>()))
                .Returns(categoria);
            _categoriaRepositoryMock.Setup(x => x.CadastrarCategoria(It.IsAny<Categoria>()))
                .Returns(categoria);

            var categoriaHandler = CreateCategoriaHandler();
            CancellationToken cancellationToken = default;         

            // Act
            var result = await categoriaHandler.Handle(command, cancellationToken);

            // Assert
            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Não deve ser possível cadastrar uma categoria com retorno nulo")]
        public async void Não_Deve_Criar_Categoria_Invalida()
        {
            // Arrange
            _categoriaRepositoryMock.Setup(x => x.CadastrarCategoria(It.IsAny<Categoria>())).Returns<Categoria>(null);

            var categoriaHandler = CreateCategoriaHandler();
            CancellationToken cancellationToken = default;

            // Act
            var result = await categoriaHandler.Handle(CreateCategoriaCommandFactory.CreateCategoriaCommand(), cancellationToken);

            // Assert
            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Deve ser possível atualizar uma categoria existente")]
        public async void Testa_Atualiza_Categoria()
        {
            // Arrange
            Categoria categoria = CategoriaFactory.Create();

            _categoriaRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(categoria);
            _mapperMock.Setup(x => x.Map(It.IsAny<UpdateCategoriaCommand>(), It.IsAny<Categoria>()))
                .Returns(categoria);
            _categoriaRepositoryMock.Setup(x => x.EditarCategoria(It.IsAny<Categoria>())).Returns(categoria);

            CancellationToken cancellationToken = default;

            var categoriaHandler = CreateCategoriaHandler();

            // Act
            var result = await categoriaHandler.Handle(UpdateCategoriaCommandFactory.UpdateCategoriaCommand(), cancellationToken);

            //Assert
            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Não deve ser possível atualizar uma categoria pois é inexistente (nula)")]
        public async void Testa_Atualiza_Categoria_Inexistente()
        {
            // Arrange
            Categoria categoria = null;

            _categoriaRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(categoria);
            _mapperMock.Setup(x => x.Map(It.IsAny<UpdateCategoriaCommand>(), It.IsAny<Categoria>()))
                .Returns(categoria);
            _categoriaRepositoryMock.Setup(x => x.EditarCategoria(It.IsAny<Categoria>())).Returns(categoria);

            UpdateCategoriaCommand request = new() { Nome = "Teste" };
            CancellationToken cancellationToken = default;

            var categoriaHandler = CreateCategoriaHandler();

            // Act
            var result = await categoriaHandler.Handle(request, cancellationToken);

            //Assert
            Assert.Equal("Categoria não localizada", result.Message);
        }

    }
}
