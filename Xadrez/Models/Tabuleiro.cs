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

    /// <summary>
    /// Distribui os peões pretos e brancos em suas posições iniciais no tabuleiro.
    /// Limpa todas as casas e peças existentes antes da distribuição.
    /// </summary>
    public void DistribuiPecas()
    {
        // Limpa casas e peças
        Casas.ForEach(c => c.Peca = null);
        Pecas.Clear();

        // Peões pretos na linha 7 (índice 6)
        for (int col = 0; col < 8; col++)
        {
            var peaoPreto = new Peao(false);
            var casa = Casas.First(c => c.Linha == 6 && c.Coluna == col);
            casa.Peca = peaoPreto;
            Pecas.Add(peaoPreto);
        }

        // Peões brancos na linha 2 (índice 1)
        for (int col = 0; col < 8; col++)
        {
            var peaoBranco = new Peao(true);
            var casa = Casas.First(c => c.Linha == 1 && c.Coluna == col);
            casa.Peca = peaoBranco;
            Pecas.Add(peaoBranco);
        }
    }

    /// <summary>
    /// Valida se o movimento informado é válido para o jogador atual.
    /// Verifica se a peça pertence ao jogador e se o destino está entre os movimentos possíveis.
    /// </summary>
    /// <param name="jogador">O jogador que está tentando mover a peça.</param>
    /// <param name="movimento">O movimento a ser validado.</param>
    /// <returns>True se o movimento for válido; caso contrário, false.</returns>
    public bool ValidaMovimento(Jogador jogador, Movimento movimento)
    {
        // Verifica se a peça pertence ao jogador
        if (movimento.Peca.EBranca != jogador.EBranco)
            return false;

        // Verifica se o movimento está entre os possíveis da peça
        var movimentosPossiveis = movimento.Peca.MovimentosPossiveis(this);
        return movimentosPossiveis.Any(m =>
            m.CasaDestino.Linha == movimento.CasaDestino.Linha &&
            m.CasaDestino.Coluna == movimento.CasaDestino.Coluna); 
    
    }

    /// <summary>
    /// Executa o movimento de uma peça no tabuleiro, removendo peças capturadas se necessário.
    /// Também verifica se um peão atingiu o fim do tabuleiro e promove a peça automaticamente.
    /// </summary>
    /// <param name="movimento">O movimento a ser executado.</param>
    public void ExecutaMovimento(Movimento movimento)
    {
        // Atualiza casas
        movimento.CasaOrigem.Peca = null;
        if (movimento.CasaDestino.Peca != null)
        {
            Pecas.Remove(movimento.CasaDestino.Peca);
            PecasCapturadas.Add(movimento.CasaDestino.Peca);
        }
            
        movimento.CasaDestino.Peca = movimento.Peca;

        // Marca que a peça foi movimentada
        movimento.Peca.FoiMovimentada = true;

        if(movimento.Peca is Peao peao)
        {
            Console.WriteLine($"Peão movido");
            peao.Promover(this);
        }
    }

    /// <summary>
    /// Reverte um movimento executado anteriormente, restaurando a peça à sua posição original
    /// e reintegrando qualquer peça que tenha sido capturada durante o movimento.
    /// </summary>
    /// <param name="movimento">O movimento a ser revertido.</param>
    public void ReverteMovimento(Movimento movimento)
    {
        // Retorna peça para casa de origem
        movimento.CasaOrigem.Peca = movimento.Peca;
        movimento.CasaDestino.Peca = movimento.PecaCapturada;

        // Se houver peça capturada, restaura no tabuleiro
        if (movimento.PecaCapturada != null)
        {
            Pecas.Add(movimento.PecaCapturada);
            PecasCapturadas.Remove(movimento.PecaCapturada);
        }

        movimento.Peca.FoiMovimentada = false;
    }

    /// <summary>
    /// Retorna a casa do tabuleiro onde a peça informada está posicionada.
    /// </summary>
    /// <param name="peca">A peça cuja posição será buscada.</param>
    /// <returns>A casa que contém a peça, ou null se não estiver no tabuleiro.</returns>
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