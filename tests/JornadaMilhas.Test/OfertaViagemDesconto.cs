using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class OfertaViagemDesconto
    {
        [Fact]
        public void RetornaPrecoAtualizadoQuandoAplicadoDesconto()
        {
            // Arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024,05,01), new DateTime(2024,05,10));
            double precoOriginal = 100;
            double desconto = 20.00;
            double precoComDesconto = precoOriginal - desconto;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //Act
            oferta.Desconto = desconto;

            //Assert
            Assert.Equal(precoComDesconto, oferta.Preco);
        }

        [Theory]
        [InlineData(120, 30)]
        [InlineData(100, 30)]
        public void RetornaDescontoMaximoQuandoValorDescontoMaiorOuIgualAoPreco(double desconto, double precoComDesconto)
        {
            // Arrange
            Rota rota = new Rota("OrigemA", "DestinoB");
            Periodo periodo = new Periodo(new DateTime(2024, 05, 01), new DateTime(2024, 05, 10));
            double precoOriginal = 100;
            OfertaViagem oferta = new OfertaViagem(rota, periodo, precoOriginal);

            //Act
            oferta.Desconto = desconto;

            //Assert
            Assert.Equal(precoComDesconto, oferta.Preco, 0.001);
        }

        [Fact]
        public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoEPrecoSaoInvalidos()
        {
            // Arrange
            int quantidadeEsperada = 3;
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 6, 1), new DateTime(2024, 5, 10));
            double preco = -100;

            //Act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //Assert
            Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
        }
    }
}
