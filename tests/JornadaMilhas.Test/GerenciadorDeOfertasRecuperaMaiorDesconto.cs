using Bogus;
using JornadaMilhasV1.Gerencidor;
using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Test
{
    public class GerenciadorDeOfertasRecuperaMaiorDesconto
    {
        [Fact]
        public void RetornaOfertaNulaQuandoListaEstaVazia()
        {
            //Arrange
            List<OfertaViagem> lista = new List<OfertaViagem>();
            GerenciadorDeOfertas gerenciador = new GerenciadorDeOfertas(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");

            // Act
            var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

            //Assert
            Assert.Null(oferta);
        }

        [Fact]
        // destino = São Paulo, desconto = 40, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            //Arrange
            Faker<Periodo> fakerPeriodo = new Faker<Periodo>()
                .CustomInstantiator(f =>
                {
                    DateTime dataInicio = f.Date.Soon();
                    return new Periodo(dataInicio, dataInicio.AddDays(30));
                });

            Rota rota = new Rota("Vitoria", "São Paulo");

            Faker<OfertaViagem> fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    fakerPeriodo.Generate(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            OfertaViagem ofertaEscolhida = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
            {
                Desconto = 40,
                Ativa = true
            };

            OfertaViagem ofertaInativa = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
            {
                Desconto = 40,
                Ativa = false
            };

            List<OfertaViagem> lista = fakerOferta.Generate(200);
            lista.Add(ofertaEscolhida);
            lista.Add(ofertaInativa);

            GerenciadorDeOfertas gerenciador = new GerenciadorDeOfertas(lista);
            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            double precoEsperado = 40.00;

            // Act
            var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

            //Assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }
    }
}