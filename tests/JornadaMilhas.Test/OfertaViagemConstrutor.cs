using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class OfertaViagemTest
    {
        [Theory]
        [InlineData("", null, "2035-01-01", "2024-01-02", 0, false)]
        [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
        [InlineData(null, "S�o Paulo", "2024-01-01", "2024-01-02", -1, false)]
        [InlineData("Vit�ria", "S�o Paulo", "2024-01-01", "2024-01-01", 0, false)]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -500, false)]
        public void RetornaEhValidoDeAcordoComDadosDeEntrada(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
        {
            // Cen�rio - Arrange
            Rota rota = new Rota(origem, destino);
            Periodo periodo = new Periodo(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));

            // A��o - Act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            // Valida��o - Assert
            Assert.Equal(validacao, oferta.EhValido);
        }        
        
        [Fact]
        public void RetornaMensagemDeErroDeRotaOuPeriodoInvalidosQuandoRotaNula()
        {
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.0;
            string erroEsperado = "A oferta de viagem n�o possui rota ou per�odo v�lidos.";

            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            Assert.Contains(erroEsperado, oferta.Erros.Sumario); // Verifica se tem o erro esperado 
            Assert.False(oferta.EhValido); // Verifica se a oferta n�o � valida
        }

        [Fact]
        public void RetornaMensagemDeErroDeDataInvalidaQuandoDataFinalMenorQueInicial()
        {
            //arrange
            Rota rota = new Rota("OrigemTeste", "DestinoTeste");
            Periodo periodo = new Periodo(new DateTime(2024, 2, 5), new DateTime(2024, 2, 1));
            double preco = 100.0;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Contains("Erro: Data de ida n�o pode ser maior que a data de volta.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-250)]
        public void RetornaMensagemDeErroDePrecoInvalidoQuandoPrecoMenorQueZero(double preco)
        {
            Rota rota = new Rota("OrigemTeste", "DestinoTeste");
            Periodo periodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));

            string erroEsperado = "O pre�o da oferta de viagem deve ser maior que zero.";

            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            Assert.Contains(erroEsperado, oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
            
        }
    }
}