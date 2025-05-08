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
    public List<Casa> Casas { get; set; } = new();

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

    /// <summary>
    /// Distribui todas as peças nas posições iniciais do tabuleiro de xadrez.
    /// </summary>
    private void DistribuiPecas(List<Casa> Casas)
    {
        
    }

    /// <summary>
    /// Valida se um movimento é permitido no tabuleiro.
    /// </summary>
    /// <param name="movimento">Movimento a ser validado.</param>
    /// <returns>Retorna verdadeiro se o movimento é válido, caso contrário, falso.</returns>
    public bool ValidaMovimento(Movimento movimento)
    {
        if (movimento == null || movimento.CasaOrigem == null || movimento.CasaDestino == null)
            return false;

        var movimentosPossiveis = movimento.CasaOrigem.Peca?.MovimentosPossiveis(this);

        if (movimentosPossiveis == null)
            return false;

        foreach (var mov in movimentosPossiveis)
        {
            if (mov.CasaDestino == movimento.CasaDestino)
                return true;
        }

        return false;
    }


    /// <summary>
    /// Executa um movimento no tabuleiro.
    /// </summary>
    /// <param name="movimento">Movimento a ser executado.</param>
    public void ExecutaMovimento(Movimento movimento)
    {
        if (!ValidaMovimento(movimento))
            return;

        movimento.CasaDestino.Peca = movimento.CasaOrigem.Peca;
        movimento.CasaOrigem.Peca = null;
    }


    /// <summary>
    /// Reverte um movimento no tabuleiro (desfaz o último movimento).
    /// </summary>
    /// <param name="movimento">Movimento a ser revertido.</param>
    public void ReverteMovimento(Movimento movimento)
    {
        if (movimento == null)
            return;

        movimento.CasaOrigem.Peca = movimento.PecaMovida;
        movimento.CasaDestino.Peca = movimento.PecaCapturada;
    }


    /// <summary>
    /// Obtém a casa atual de uma peça no tabuleiro.
    /// </summary>
    /// <param name="peca">Peça a ser localizada.</param>
    /// <returns>A casa onde a peça se encontra, ou null se não encontrada.</returns>
    public Casa ObtemCasaPeca(Peca peca)
    {
        
    }


    //Grupo 8 - Feito
    public bool VerificaXeque(bool eBranca)
    {
        // Seleciona as peças do jogador conforme a cor informada
        var pecasDoJogador = eBranca ? PecasBrancas : PecasPretas;

        // Encontra o rei do jogador
        IPeca? rei = pecasDoJogador.FirstOrDefault(p => p is IRei);
        if (rei == null)
        {
            // Não encontrou o rei — talvez o jogo esteja em um estado inválido
            return false;
        }

        // Encontra a casa onde está o rei
        Casa? casaDoRei = ObtemCasaPeca(rei);
        if (casaDoRei == null)
        {
            // O rei não está posicionado em nenhuma casa — também um estado inválido
            return false;
        }

        // Seleciona as peças inimigas
        var pecasInimigas = eBranca ? PecasPretas : PecasBrancas;

        // Verifica se alguma peça inimiga pode capturar o rei
        foreach (var pecaInimiga in pecasInimigas)
        {
            var movimentosInimigos = pecaInimiga.MovimentosPossiveis(this);
            foreach (var movimento in movimentosInimigos)
            {
                if (movimento.CasaDestino == casaDoRei)
                {
                    // O rei está sob ameaça
                    return true;
                }
            }
        }

        // Nenhuma peça inimiga ameaça o rei
        return false;
    }

    public bool VerificaXequeMate(bool eBranca)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica se uma casa está sob ataque.
    /// </summary>
    /// <param name="casa">Casa a ser verificada.</param>
    /// <param name="eBranca">Indica se o jogador é branco ou preto.</param>
    /// <returns>Retorna verdadeiro se a casa está sob ataque, caso contrário, falso.</returns>
    public bool VerificaPerigo(Casa casa, bool eBranca)
    {
        foreach (var linha in Casas)
        {
            foreach (var outraCasa in linha)
            {
                if (outraCasa.Peca != null && outraCasa.Peca.EBranca != eBranca)
                {
                    var movimentosPossiveis = outraCasa.Peca.MovimentosPossiveis(this);
                    foreach (var movimento in movimentosPossiveis)
                    {
                        if (movimento.Destino == casa)
                            return true;
                    }
                }
            }
        }
        return false;
    }
}