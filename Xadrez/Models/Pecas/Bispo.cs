
using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Bispo(bool eBranca) : Peca(eBranca), IBispo
{
    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoPecas(this, tabuleiro, false, false, true, true);
    }
}
