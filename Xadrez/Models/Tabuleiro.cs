using System.Data.Common;
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

    public void DistribuiPecas()
    {
        // Adicionando peças brancos
        AdicionarPecas(true);

        // Adicionando peças pretas
        AdicionarPecas(false);
    }

    public void AdicionarPecas(Boolean eBraca)
    {
        // Adicionando peões brancos
        for (int coluna = 0; coluna < 8; coluna++)
        {
            this.Casas.Find(casa => casa.Linha == 1 && casa.Coluna == coluna).Peca = new Peao(eBraca);
        }

        // Adicionando demais peças brancas
        for (int coluna = 0; coluna < 8; coluna++)
        {
            if (coluna == 0 || coluna == 7)
            {
                this.Casas.Find(casa => casa.Linha == 0 && casa.Coluna == coluna).Peca = new Torre(eBraca);
            }
            else if (coluna == 1 || coluna == 6)
            {
                this.Casas.Find(casa => casa.Linha == 0 && casa.Coluna == coluna).Peca = new Cavalo(eBraca);
            }
            else if (coluna == 2 || coluna == 5)
            {
                this.Casas.Find(casa => casa.Linha == 0 && casa.Coluna == coluna).Peca = new Bispo(eBraca);
            }
            else if (coluna == 3)
            {
                this.Casas.Find(casa => casa.Linha == 0 && casa.Coluna == coluna).Peca = new Rainha(eBraca);
            }
            else
            {
                this.Casas.Find(casa => casa.Linha == 0 && casa.Coluna == coluna).Peca = new Rei(eBraca);
            }
        }
    }

    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        bool eCasaDestinoLinha = movimento.CasaDestino.Linha >= 0 && movimento.CasaDestino.Linha < 8;
        bool eCasaDestinoColuna = movimento.CasaDestino.Coluna >= 0 && movimento.CasaDestino.Coluna < 8;
        bool eCasaOrigemLinha = movimento.CasaOrigem.Linha >= 0 && movimento.CasaOrigem.Linha < 8;
        bool eCasaOrigemColuna = movimento.CasaOrigem.Coluna >= 0 && movimento.CasaOrigem.Coluna < 8;
        bool eMovimentoPossivel = movimento.Peca.MovimentosPossiveis(this).Contains(movimento);

        if (eCasaDestinoLinha && eCasaDestinoColuna && eCasaOrigemLinha && eCasaOrigemColuna && eMovimentoPossivel) {
            IPeca? pecaCapturada = movimento.PecaCapturada;
            IPeca? pecaCasaDestino = movimento.CasaDestino.Peca;
            if (pecaCapturada != null && pecaCasaDestino != null)
            {
                if(pecaCapturada.EBranca != pecaCasaDestino.EBranca)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void ExecutaMovimento(Movimento movimento)
    {
        Casa? casaDestino = Casas.Find(casa => casa.Linha == movimento.CasaDestino.Linha && casa.Coluna == movimento.CasaDestino.Coluna);
        if (casaDestino != null )
        {
            IPeca pecaMorta = casaDestino.Peca;
            IPeca pecaAssasina = movimento.PecaCapturada;
            if (pecaMorta == pecaAssasina) 
            {
                if(pecaMorta.EBranca)
                {
                    this.PecasBrancasCapturadas.Add(pecaMorta);
                }
                else
                {
                    this.PecasPretasCapturadas.Add(pecaMorta);
                }
            }
            casaDestino.Peca = movimento.Peca;
            movimento.CasaOrigem.Peca = null;
        }
    }

    public void ReverteMovimento(Movimento movimento)
    {
        ExecutaMovimento(new Movimento(movimento.Peca, movimento.CasaDestino, movimento.CasaOrigem, movimento.PecaCapturada, movimento.ERoque));
    }

    public Casa? ObtemCasaPeca(IPeca peca)
    {
        return Casas.FirstOrDefault(c => c.Peca == peca);
    }

    public bool VerificaXeque(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        throw new NotImplementedException();
    }
}