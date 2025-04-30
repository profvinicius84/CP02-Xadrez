using Xadrez.Models.Pecas;

namespace Xadrez.Models;

/// <summary>
/// Representa um movimento no jogo de xadrez.
/// </summary>
/// <param name="peca">Representa a peça que está sendo movida.</param>
/// <param name="casaOrigem">Representa a casa de origem da peça.</param>
/// <param name="casaDestino">Representa a casa de destino da peça.</param>
/// <param name="pecaCapturada">Representa a peça capturada, se houver.</param>
public class Movimento(IPeca peca, Casa casaOrigem, Casa casaDestino, IPeca? pecaCapturada = null, bool eRoque = false, bool ativaPromocao = false, bool enPassant = false)
{    
    /// <summary>
    /// Representa a peça que está sendo movida.
    /// </summary>
    public IPeca Peca { get;} = peca;

    /// <summary>
    /// Representa a casa de origem da peça.
    /// </summary>
    public Casa CasaOrigem { get;} = casaOrigem;

    /// <summary>
    /// Representa a casa de destino da peça.
    /// </summary>
    public Casa CasaDestino { get; } = casaDestino;

    /// <summary>
    /// Representa a peça capturada, se houver.
    /// </summary>
    public IPeca? PecaCapturada { get;} = pecaCapturada;

    /// <summary>
    /// Indica se o movimento é um roque ou não.
    /// </summary>
    public bool ERoque { get; } = eRoque;

    /// <summary>
    /// Indica se o movimento ativa uma promoção de peão.
    /// </summary>
    public bool AtivaPromocao { get; } = ativaPromocao;

    /// <summary>
    /// Indica se o movimento é um en passant.
    /// </summary>
    public bool EnPassant { get; } = enPassant;

    /// <summary>
    /// Representa a notação algébrica do movimento.
    /// </summary>
    public string NotacaoAlgebrica
    {
        get
        {
            if (ERoque)
                return $"O-O{(CasaDestino.Coluna == 2 ? "-O" : "")}";
            if(AtivaPromocao)
                return $"{CasaOrigem.Codigo.ToLower()}{(PecaCapturada is not null ? "x" : "")}{CasaDestino.Codigo.ToLower()}{Peca.Codigo}";
            return $"{Peca.Codigo}{CasaOrigem.Codigo.ToLower()}{(PecaCapturada is not null ? "x" : "")}{CasaDestino.Codigo.ToLower()}{(EnPassant ? " e.p." : "")}";
        }
    }
}