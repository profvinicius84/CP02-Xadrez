using Microsoft.AspNetCore.Components.Web;
using System.Text.Json;
using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa o tabuleiro de xadrez.
/// </summary>
public class Tabuleiro : ITabuleiro
{   
    /// <summary>
    /// Representa o tamanho do tabuleiro de xadrez.
    /// </summary>
    public List<Casa> Casas { get; } = new();

    /// <summary>
    /// Pecas das partida
    /// </summary>
    public List<IPeca> Pecas { get; set; } = new();

    /// <summary>
    /// Pecas brancas da partida
    /// </summary>
    public List<IPeca> PecasBrancas => Pecas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Pecas pretas da partida
    /// </summary>
    public List<IPeca> PecasPretas => Pecas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasCapturadas { get; set; } = new();

    /// <summary>
    /// Representa a lista de peças brancas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasBrancasCapturadas => PecasCapturadas.Where(p => p.EBranca).ToList();

    /// <summary>
    /// Representa a lista de peças pretas capturadas durante a partida.
    /// </summary>
    public List<IPeca> PecasPretasCapturadas => PecasCapturadas.Where(p => !p.EBranca).ToList();

    /// <summary>
    /// Obtém quantidade de movimentos realizados na partida.
    /// </summary>
    public int MovimentosExecutados { private set; get; }

    /// <summary>
    /// Construtor que inicializa o tabuleiro de xadrez com 64 casas.
    /// </summary>
    public Tabuleiro()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var casa = new Casa(i, j);
                Casas.Add(casa);
            }
        }
    }

    /// <summary>
    /// Distribui as peças no tabuleiro de xadrez. As peças brancas são colocadas nas linhas 1 e 2, enquanto as peças pretas são colocadas nas linhas 7 e 8.
    /// </summary>
    public void DistribuiPecas()
    {
        //Peças brancas
        Casas[0].Peca = new Torre(true);
        Casas[1].Peca = new Cavalo(true);
        Casas[2].Peca = new Bispo(true);        
        Casas[3].Peca = new Rainha(true);
        Casas[4].Peca = new Rei(true);
        Casas[5].Peca = new Bispo(true);
        Casas[6].Peca = new Cavalo(true);
        Casas[7].Peca = new Torre(true);

        for (int i = 8; i < 16; i++)
        {
            Casas[i].Peca = new Peao(true);
        }

        //Peças pretas
        Casas[63].Peca = new Torre(false);
        Casas[62].Peca = new Cavalo(false);
        Casas[61].Peca = new Bispo(false);       
        Casas[60].Peca = new Rei(false);
        Casas[59].Peca = new Rainha(false);
        Casas[58].Peca = new Bispo(false);
        Casas[57].Peca = new Cavalo(false);
        Casas[56].Peca = new Torre(false);

        for (int i = 48; i < 56; i++)
        {
            Casas[i].Peca = new Peao(false);
        }

        Pecas.AddRange(Casas.Where(c => c.Peca is not null).Select(c => c.Peca));
    }

    /// <summary>
    /// Obtém a casa com base nas coordenadas (linha e coluna) do tabuleiro. As coordenadas são baseadas em zero, ou seja, a primeira linha e coluna são 0.
    /// </summary>
    /// <param name="linha">Linha da casa no tabuleiro. A linha é baseada em zero, ou seja, a primeira linha é 0.</param>
    /// <param name="coluna">Coluna da casa no tabuleiro. A coluna é baseada em zero, ou seja, a primeira coluna é 0.</param>
    /// <returns>Retorna a casa correspondente às coordenadas fornecidas. Se a casa não existir (fora dos limites do tabuleiro), retorna nulo.</returns>
    public Casa? ObtemCasaCoordenadas(int linha, int coluna)
    {
        if (linha < 0 || linha > 7 || coluna < 0 || coluna > 7)
            return null;
        return Casas.FirstOrDefault(c => c.Linha == linha && c.Coluna == coluna);
    }

    public bool ValidaMovimento(Movimento movimento, Jogador? jogador = null)
    {   
        if(jogador is not null && jogador.EBranco != movimento.Peca.EBranca)
            return false;
        if(PecasCapturadas.Contains(movimento.Peca) || PecasCapturadas.Contains(movimento.PecaCapturada))
            return false;
        if(!Casas.Contains(movimento.CasaOrigem) || !Casas.Contains(movimento.CasaDestino))
            return false;
        if (movimento.Peca != movimento.CasaOrigem.Peca)
            return false;
        if (movimento.PecaCapturada is not null) {
            if(movimento.PecaCapturada.EBranca == movimento.Peca.EBranca)
                return false;
            if(!movimento.EnPassant && movimento.PecaCapturada != movimento.CasaDestino.Peca)
                return false;
            if(movimento.Peca is IRei && movimento.PecaCapturada is IRei)
                return false;
        }
        if (movimento.ERoque)
        {
            if (movimento.Peca is not IRei)
                return false;
            if(!(movimento.Peca as IRei).VerificaRoque(this) && !(movimento.Peca as IRei).VerificaRoque(this, true))
                return false;
        }
        if (movimento.EnPassant)
        {
            if (movimento.Peca is not IPeao)
                return false;
            if (movimento.PecaCapturada is null)
                return false;
            if (movimento.PecaCapturada is not IPeao)
                return false;
            if (movimento.CasaDestino.Linha != movimento.CasaOrigem.Linha + (movimento.Peca.EBranca ? 1 : -1))
                return false;
            if(movimento.CasaDestino.Peca is not null)
                return false;
        }
        if (movimento.AtivaPromocao && movimento.CasaDestino.Linha != (movimento.Peca.EBranca ? 7 : 0))
            return false;

        return SimulaMovimento(movimento);
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        if (movimento.PecaCapturada is not null)
        {
            if (movimento.EnPassant)
            {
                var casaEnpassant = ObtemCasaPeca(movimento.PecaCapturada);
                casaEnpassant.Peca = null;
            }
            else
            {
                movimento.CasaDestino.Peca = null;
            }
            PecasCapturadas.Add(movimento.PecaCapturada);
        }        
        movimento.CasaDestino.Peca = movimento.Peca;
        movimento.CasaOrigem.Peca = null;
        if (movimento.ERoque)
        {
            if (movimento.CasaDestino.Coluna == 6) //Roque pequeno
            {
                var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 7);
                ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 5).Peca = casaTorre.Peca;
                casaTorre.Peca = null;
            }
            else //Roque grande
            {
                var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 0);
                ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 3).Peca = casaTorre.Peca;
                casaTorre.Peca = null;
            }
        }
        if (movimento.Peca is IPeao && movimento.AtivaPromocao)
            (movimento.Peca as IPeao).Promover(new Rainha(movimento.Peca.EBranca));
        MovimentosExecutados++;
        movimento.Peca.MovimentosRelizados++;
        movimento.Peca.UltimoMovimento = MovimentosExecutados;
    }

    public void ReverteMovimento(Movimento movimento)
    {
        movimento.CasaOrigem.Peca = movimento.Peca;
        movimento.CasaDestino.Peca = null;
        if (movimento.PecaCapturada is not null)
        {
            if (movimento.EnPassant)
            {
                var casaEnPassant = ObtemCasaCoordenadas(movimento.CasaDestino.Linha - (movimento.Peca.EBranca ? 1 : -1), movimento.CasaDestino.Coluna);
                casaEnPassant.Peca = movimento.PecaCapturada;
            }
            else
            {
                movimento.CasaDestino.Peca = movimento.PecaCapturada;
            }
            PecasCapturadas.Remove(movimento.PecaCapturada);
        }
        if (movimento.ERoque)
        {
            if (movimento.CasaDestino.Coluna == 6) //Roque pequeno
            {
                var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 5);
                ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 7).Peca = casaTorre.Peca;
                casaTorre.Peca = null;
            }
            else //Roque grande
            {
                var casaTorre = ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 3);
                ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 0).Peca = casaTorre.Peca;
                casaTorre.Peca = null;
            }
        }
        if (movimento.Peca is IPeao && movimento.AtivaPromocao)
            (movimento.Peca as IPeao).Despromover();
        MovimentosExecutados--;
        movimento.Peca.MovimentosRelizados--;
    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca || (c.Peca is IPeao && (c.Peca as IPeao).PecaPromocao == peca));
    }

    public bool VerificaXeque(bool eBranca)
    {
        return ObtemMovimentosAtaqueCheque(eBranca).Count > 0;
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        Casa casaRei = Casas.FirstOrDefault(c => c.Peca is IRei rei && rei.EBranca == eBranca);
        List<Movimento> movimentos = new();
        foreach (var peca in eBranca ? PecasBrancas : PecasPretas)
        {
            if (!PecasCapturadas.Contains(peca))
                movimentos.AddRange(peca.MovimentosPossiveis(this));
        }

        movimentos.RemoveAll(m => !ValidaMovimento(m));

        return movimentos.Count == 0;
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        List<Movimento> movimentos = new();
        foreach (var peca in eBranca ? PecasPretas : PecasBrancas)
        {
            movimentos.AddRange(peca.MovimentosPossiveis(this));
        }
        return movimentos.Any(m => m.CasaDestino == casa);
    }

    /// <summary>
    /// Obtém movimentos quem mantém o rei em xeque.
    /// </summary>    
    /// <returns>Uma lista de movimentos que mantém o rei em xeque.</returns>
    public List<Movimento> ObtemMovimentosAtaqueCheque(bool eBranca)
    {
        Casa casaRei = Casas.FirstOrDefault(c => c.Peca is IRei rei && rei.EBranca == eBranca);
        List<Movimento> movimentos = new();
        foreach (var peca in eBranca ? PecasPretas : PecasBrancas)
        {
            if(!PecasCapturadas.Contains(peca))
                movimentos.AddRange(peca.MovimentosPossiveis(this));
        }
        return movimentos.FindAll(m => m.CasaDestino == casaRei);
    }

    protected bool SimulaMovimento(Movimento movimento)
    {
        var ultimoMovimentoPeca = movimento.Peca.UltimoMovimento;
        
        ExecutaMovimento(movimento);

        bool resultado = !VerificaXeque(movimento.Peca.EBranca);

        ReverteMovimento(movimento);
        movimento.Peca.UltimoMovimento = ultimoMovimentoPeca;

        return resultado;
    }
}