using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] GameObject battleResult;
    [SerializeField] TMP_Text battleResultText;
    [SerializeField] Player player1;
    [SerializeField] Player player2;

    
    [SerializeField] bool isReturningDone;
    [SerializeField] bool isPlayerEleminated;

    enum State
    {
        Preparation,
        Player1Select,
        Player2Select,
        Attacking,
        Damaging,
        Returning,
        BattleIsOver
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Preparation:
                player1.Prepare();
                player2.Prepare();

                player1.SetPlay(true);
                player2.SetPlay(false);
                state = State.Player1Select;
                break;
            case State.Player1Select:
            if (player1.SelectedCharacter != null)
            {
                player1.SetPlay(false);
                player2.SetPlay(true);
                state = State.Player2Select;
            }
                break;
            case State.Player2Select:
            if (player2.SelectedCharacter != null)
            {
                player2.SetPlay(false);
                player1.Attack();
                player2.Attack();
                state = State.Attacking;
            }
                break;
            case State.Attacking:
            if (player1.IsAttacking() == false && player2.IsAttacking() == false)
            {
                CalculateBattle(player1, player2, out Player winner, out Player loser);
                if (loser == null)
                {
                    player1.TakeDamage(player2.SelectedCharacter.AttackPower);
                    player2.TakeDamage(player1.SelectedCharacter.AttackPower);
                }
                else
                {
                    loser.TakeDamage(winner.SelectedCharacter.AttackPower);
                }

                    state = State.Damaging;
                }
                break;

            case State.Damaging:
                if (player1.IsDamaging() == false && player2.IsDamaging() == false)
                {
                    if (player1.SelectedCharacter.CurrentHP == 0)
                    {
                        player1.Remove(player1.SelectedCharacter);
                    }
                    if (player2.SelectedCharacter.CurrentHP == 0)
                    {
                        player2.Remove(player2.SelectedCharacter);
                    }

                    // animasi return
                    if (player1.SelectedCharacter != null)
                    {
                        player1.Return();   
                    }

                    if (player2.SelectedCharacter != null)
                    {
                     player2.Return();   
                    }

                    state = State.Returning;
                }
                break;

            case State.Returning:
                if (player1.isReturning () == false && player2.isReturning() == false )
                {
                    if (player1.CharacterList.Count == 0 && player2.CharacterList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Battle is Over!\nDraw!";
                        state = State.BattleIsOver; 
                        
                    }

                    else if (player1.CharacterList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Battle is Over!\nEnemy win!";
                        state = State.BattleIsOver;
                        
                    }
                    else if (player2.CharacterList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Battle is Over!\nYou win!";
                        state = State.BattleIsOver;
                        
                    }
                    else
                    {
                        
                        state = State.Preparation;
                    }
                }
                break;
            case State.BattleIsOver:
                Debug.Log("Battle is Over. player1: " + player1.CharacterList.Count + "player2 :" + player2.CharacterList.Count);
                break;
        }
    }

    private void CalculateBattle(Player player1, Player player2, out Player winner, out Player loser)
    {
        var type1 = player1.SelectedCharacter.Type;
        var type2 = player2.SelectedCharacter.Type;

        if(type1 == CharacterType.Rock && type2 == CharacterType.Paper)
        {
            winner = player2;
            loser = player1;
        }

        else if(type1 == CharacterType.Rock && type2 == CharacterType.Scissor)
        {
            winner = player1;
            loser = player2;
        }

        else if(type1 == CharacterType.Paper && type2 == CharacterType.Rock)
        {
            winner = player1;
            loser = player2;
        }

        else if(type1 == CharacterType.Paper && type2 == CharacterType.Scissor)
        {
            winner = player2;
            loser = player1;
        }

        else if(type1 == CharacterType.Scissor && type2 == CharacterType.Rock)
        {
            winner = player2;
            loser = player1;
        }

        else if(type1 == CharacterType.Scissor && type2 == CharacterType.Paper)
        {
            winner = player1;
            loser = player2;
        }
        else
        {
            winner = null;
            loser = null;
        }
    }   

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Main");
    }

}
