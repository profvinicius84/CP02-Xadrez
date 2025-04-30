using Xadrez.Models.Pecas;

namespace Xadrez.Models.Estrategias;

public class EstrategiaMovimentoRei(IRei peca, ITabuleiro tabuleiro) : EstrategiaMovimentoPecas(peca, tabuleiro, true, true, true, true, 1)
{
    public override List<Movimento> GeraMovimentos()
    {
        var movimentos = base.GeraMovimentos();

        if (!Peca.FoiMovimentada)
        {
            var casaRei = Tabuleiro.ObtemCasaPeca(Peca);
            if (PodeFazerRoquePequeno(casaRei))
            {                
                var casaDestino = Tabuleiro.ObtemCasaCoordenadas(casaRei.Linha, 6); // Casa de destino para o roque pequeno
                movimentos.Add(new Movimento(Peca, casaRei, casaDestino, eRoque: true));
            }
            if(PodeFazerRoqueGrande(casaRei))
            {
                var casaDestino = Tabuleiro.ObtemCasaCoordenadas(casaRei.Linha, 2); // Casa de destino para o roque grande
                movimentos.Add(new Movimento(Peca, casaRei, casaDestino, eRoque: true));
            }
        }       

        return movimentos;
    }

    /// <summary>
    /// Verifica se o rei pode realizar o roque pequeno (lado do rei).
    /// </summary>
    /// <param name="casaRei">A casa de origem do rei.</param>
    /// <returns>Retorna verdadeiro se o rei pode fazer o roque pequeno, caso contrário, retorna falso.</returns>
    public bool PodeFazerRoquePequeno(Casa casaRei)
    {
        var casaTorre = Tabuleiro.ObtemCasaCoordenadas(casaRei.Linha, 7); // Torre na coluna 7
                
        if (casaTorre?.Peca is null || casaTorre.Peca is not ITorre || casaTorre.Peca.FoiMovimentada || casaTorre.Peca.EBranca != Peca.EBranca)
            return false;

        return !Tabuleiro.Casas.FindAll(c => c.Linha == casaRei.Linha && c.Coluna > casaRei.Coluna && c != casaTorre).Exists(c => c.Peca is not null);
    }

    /// <summary>
    /// Verifica se o rei pode realizar o roque grande (lado da dama).
    /// </summary>
    /// <param name="casaRei">A casa de origem do rei.</param>
    /// <returns>Retorna verdadeiro se o rei pode fazer o roque grande, caso contrário, retorna falso.</returns>
    public bool PodeFazerRoqueGrande(Casa casaRei)
    {
        var casaTorre = Tabuleiro.ObtemCasaCoordenadas(casaRei.Linha, 0); // Torre na coluna 7

        if (casaTorre?.Peca is null || casaTorre.Peca is not ITorre || casaTorre.Peca.FoiMovimentada || casaTorre.Peca.EBranca != Peca.EBranca)
            return false;

        return !Tabuleiro.Casas.FindAll(c => c.Linha == casaRei.Linha && c.Coluna < casaRei.Coluna && c != casaTorre).Exists(c => c.Peca is not null);
    }
}
