using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

public class EstrategiaMovimentoPeao(IPeao peca, ITabuleiro tabuleiro) : EstrategiaMovimentoPecas(peca, tabuleiro, true, false, false, false, peca.FoiMovimentada ? 1 : 2)
{
    public override List<Movimento> GeraMovimentos()
    {
        var movimentos = base.GeraMovimentos();
        movimentos.RemoveAll(m => m.PecaCapturada is not null);

        var estrategiaCaptura = new EstrategiaMovimentoPecas(peca, Tabuleiro, false, false, true, false, 1);
        movimentos.AddRange(estrategiaCaptura.GeraMovimentos().Where(m => m.PecaCapturada is not null));

        int linhaFinal = peca.EBranca ? 7 : 0;

        foreach (var movimento in movimentos.FindAll(m => m.CasaDestino.Linha == linhaFinal))
        {
            var novoMovimento = new Movimento(movimento.Peca, movimento.CasaOrigem, movimento.CasaDestino, movimento.PecaCapturada, ativaPromocao: true);
            movimentos.Add(novoMovimento);
            movimentos.Remove(movimento);
        }

        int linhaEnPassant = Peca.EBranca ? 4 : 3;
        var casa = Tabuleiro.ObtemCasaPeca(Peca);
        if(casa.Linha == linhaEnPassant)
        {
            var casaEnPassant = Tabuleiro.Casas.FirstOrDefault(c => c.Linha == casa.Linha
                                    && (c.Coluna == casa.Coluna - 1 || c.Coluna == casa.Coluna + 1)
                                    && c.Peca is not null
                                    && c.Peca.EBranca != Peca.EBranca
                                    && c.Peca is Peao
                                    && c.Peca.MovimentosRelizados == 1
                                    && c.Peca.UltimoMovimento == Tabuleiro.MovimentosExecutados);
            if (casaEnPassant != null)
            {
                var casaDestino = Tabuleiro.ObtemCasaCoordenadas(casaEnPassant.Linha + (Peca.EBranca ? 1 : -1), casaEnPassant.Coluna);
                movimentos.Add(new Movimento(Peca, casa, casaDestino, casaEnPassant.Peca, enPassant: true));
            }
        }

        return movimentos;
    }
}
