using Xadrez.Models.Estrategias;

namespace Xadrez.Models.Pecas;

public class Peao(bool eBranca) : Peca(eBranca), IPeao
{    
    public bool VarificaPromocao(ITabuleiro tabuleiro)
    {
        var casa = tabuleiro.ObtemCasaPeca(this);
        if (casa is not null)
        {
            return (this.EBranca && casa.Linha == 7) || (!this.EBranca && casa.Linha == 0);
        }
        return false;
    }
    
    public void Promover(IPeca pecaPromocao)
    {
        if (pecaPromocao is not IPeao && pecaPromocao is not IRei && pecaPromocao.EBranca == this.EBranca)
            PecaPromocao = pecaPromocao;
    }

    public void Despromover()
    {
        PecaPromocao = null;
    }

    public override IEstrategiaMovimento ObtemEstrategiaMovimento(ITabuleiro tabuleiro)
    {
        return new EstrategiaMovimentoPeao(this, tabuleiro);
    }

    public override List<Movimento> MovimentosPossiveis(ITabuleiro tabuleiro)
    {
        if (PecaPromocao is not null)
        {
            var movimentos = PecaPromocao.MovimentosPossiveis(tabuleiro);
            
            var novosMovimentos = new List<Movimento>();

            foreach (var movimento in movimentos)
            {
                novosMovimentos.Add(new Movimento(this, movimento.CasaOrigem, movimento.CasaDestino, movimento.PecaCapturada));
            }
            return novosMovimentos;
        }
        return base.MovimentosPossiveis(tabuleiro);
    }

    public override string Codigo => PecaPromocao is not null ? PecaPromocao.Codigo : "";

    public IPeca? PecaPromocao { get; private set; }
}