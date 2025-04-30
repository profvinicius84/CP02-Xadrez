using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Torre(bool eBranca) : Peca(eBranca), ITorre
{
    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoPecas(this, tabuleiro, true, true, false, true);
    }
}