using Microsoft.AspNetCore.Components;
using Xadrez.Models;
using Xadrez.Models.Pecas;

namespace Xadrez.Components.Shared;

/// <summary>
/// Classe que representa o painel do tabuleiro de xadrez.
/// </summary>
public partial class PainelTabuleiro
{

    /// <summary>
    /// Partida de xadrez que contém o tabuleiro e as peças.
    /// </summary>
    private Models.Partida<Models.Tabuleiro> _partida;


    /// <summary>
    /// Representa a partida de xadrez associada ao painel do tabuleiro.
    /// </summary>
    [Parameter]
    public Models.Partida<Models.Tabuleiro> Partida {
        get
        {
            return _partida;
        }
        set
        {
            _partida = value;
            if(_partida is not null)            
                AtualizaCondicaoDisponibilidadeCasas();
        }
    }

    /// <summary>
    /// Evento que é chamado quando o estado do tabuleiro muda.
    /// </summary>
    [Parameter]
    public EventCallback AoMudarEstado { get; set; }

    /// <summary>
    /// Representa a peça selecionada atualmente no tabuleiro.
    /// </summary>
    public Models.Pecas.IPeca? PecaSelecionada { get; private set; }

    /// <summary>
    /// Representa a lista de movimentos possíveis para a peça selecionada.
    /// </summary>
    public List<Movimento> MovimentosPossiveisPecaSelecionada { get; } = new();

    /// <summary>
    /// Representa a lista de blocos de casa no tabuleiro.
    /// </summary>
    private Dictionary<string, BlocoCasa> BlocosCasa { get; } = new();

    /// <summary>
    /// Evento que é chamado ao selecionar uma casa no tabuleiro.
    /// </summary>
    /// <param name="casa">Representa a casa selecionada no tabuleiro.</param>
    public void CliqueCasa(Models.Casa casa)
    {
        if (PecaSelecionada is null && casa.Peca is not null && Partida.JogadorDaVez.EBranco == casa.Peca.EBranca)
        {
            var movimentos = casa.Peca.MovimentosPossiveis(Partida.Tabuleiro);
            casa.Peca.ObtemEstrategiaMovimento(Partida.Tabuleiro).ValidaMovimentos(movimentos);
            if (movimentos.Count > 0)
            {
                PecaSelecionada = casa.Peca;
                MovimentosPossiveisPecaSelecionada.AddRange(movimentos);
            }
        }
        else if (casa.Peca is not null && casa.Peca == PecaSelecionada)
        {
            PecaSelecionada = null;
            MovimentosPossiveisPecaSelecionada.Clear();
        }
        else if (MovimentosPossiveisPecaSelecionada is not null && MovimentosPossiveisPecaSelecionada.Exists(m => m.CasaDestino == casa))
        {
            var movimento = MovimentosPossiveisPecaSelecionada.First(m => m.CasaDestino == casa);

            Partida.Tabuleiro.ExecutaMovimento(movimento);            
            PecaSelecionada = null;
            MovimentosPossiveisPecaSelecionada.Clear();
            Partida.Movimentos.Push(movimento);
            AoMudarEstado.InvokeAsync();
        }
        AtualizaCondicaoDisponibilidadeCasas();
    }

    /// <summary>
    /// Atualiza a condição de disponibilidade das casas no tabuleiro.
    /// </summary>
    public void AtualizaCondicaoDisponibilidadeCasas()
    {
        if (BlocosCasa.Count > 0)
        {
            Partida.Tabuleiro.Casas.ForEach(c => BlocosCasa[c.Codigo].CondicaoDisponibilidade = "");
            if (PecaSelecionada is not null)
                BlocosCasa[Partida.Tabuleiro.ObtemCasaPeca(PecaSelecionada).Codigo].CondicaoDisponibilidade = "selecionada";
            else
                Partida.Tabuleiro.Casas.FindAll(c => c.Peca?.EBranca == Partida.JogadorDaVez.EBranco).ForEach(c => BlocosCasa[c.Codigo].CondicaoDisponibilidade = "vez");

            foreach (var movimento in MovimentosPossiveisPecaSelecionada)
            {
                var blocoCasa = BlocosCasa[movimento.CasaDestino.Codigo];
                if (movimento.PecaCapturada is not null)
                {
                    blocoCasa.CondicaoDisponibilidade = "perigo";
                    if (movimento.EnPassant)
                        BlocosCasa[Partida.Tabuleiro.ObtemCasaPeca(movimento.PecaCapturada).Codigo].CondicaoDisponibilidade += " perigo-enpassant";
                }
                else if (movimento.ERoque)
                {
                    blocoCasa.CondicaoDisponibilidade = "roque";
                    if (movimento.CasaDestino.Coluna == 6) //Roque pequeno
                    {
                        BlocosCasa[Partida.Tabuleiro.ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 5).Codigo].CondicaoDisponibilidade += " roque-torre-destino";
                        BlocosCasa[Partida.Tabuleiro.ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 7).Codigo].CondicaoDisponibilidade += " roque-torre";
                    }
                    else //Roque grande
                    {
                        BlocosCasa[Partida.Tabuleiro.ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 3).Codigo].CondicaoDisponibilidade += " roque-torre-destino";
                        BlocosCasa[Partida.Tabuleiro.ObtemCasaCoordenadas(movimento.CasaDestino.Linha, 0).Codigo].CondicaoDisponibilidade += " roque-torre";
                    }
                }
                else
                {
                    blocoCasa.CondicaoDisponibilidade += " disponivel";
                }
            }

            var casaRei = Partida.Tabuleiro.Casas.FirstOrDefault(c => c.Peca is IRei && c.Peca.EBranca == Partida.JogadorDaVez.EBranco);
            if (Partida.Tabuleiro.VerificaXequeMate(Partida.JogadorDaVez.EBranco))
            {
                int a = 1;
            }
            else if (Partida.Tabuleiro.VerificaXeque(Partida.JogadorDaVez.EBranco))
            {
                BlocosCasa[casaRei.Codigo].CondicaoDisponibilidade += " cheque";
                foreach (var movimentoCheque in Partida.Tabuleiro.ObtemMovimentosAtaqueCheque(Partida.JogadorDaVez.EBranco))
                {
                    BlocosCasa[movimentoCheque.CasaOrigem.Codigo].CondicaoDisponibilidade += " atacante-cheque";
                }
            }
        }
    }    
}