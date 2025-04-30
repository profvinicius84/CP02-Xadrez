using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Cavalo(bool eBranca) : Peca(eBranca), ICavalo
{
    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoCavalo(this, tabuleiro);
    }
}
