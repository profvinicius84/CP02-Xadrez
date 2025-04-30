using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Rainha(bool eBranca) : Peca(eBranca), IRainha
{
    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoPecas(this, tabuleiro, true, true, true, true);
    }
}