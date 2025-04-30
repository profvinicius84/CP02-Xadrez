using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Rei(bool eBranca) : Peca(eBranca), IRei
{
    public Movimento ExecutaRoque(ITabuleiro tabuleiro, bool roquePequeno = false)
    {
        throw new NotImplementedException();
    }

    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoRei(this, tabuleiro);
    }

    public bool VerificaRoque(ITabuleiro tabuleiro, bool roquePequeno = false)
    {
        var estrategia = new EstrategiaMovimentoRei(this, tabuleiro);
        return roquePequeno ? estrategia.PodeFazerRoquePequeno(tabuleiro.ObtemCasaPeca(this)) : estrategia.PodeFazerRoqueGrande(tabuleiro.ObtemCasaPeca(this));
    }

    public bool VerificaXequeMate(ITabuleiro tabuleiro)
    {
        throw new NotImplementedException();
    }
}