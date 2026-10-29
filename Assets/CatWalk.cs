using UnityEngine;
using UnityEngine.InputSystem; //Biblioteca do input novo
using UnityEngine.SceneManagement; //Biblioteca da Cena

public class CatWalk : MonoBehaviour
{
    //Declarando as variáveis do RigidBody2D e SpriteRenderer do personagem
    Rigidbody2D rb;
    SpriteRenderer sprite;

    //SerializeField serve para ver e alterar no inspector mesmo sendo privado.
    [SerializeField] private float velocidade = 5f, velpulo = 8f; //Variáveis da velocidade e de pulo

    //Booelana que verifica se pode pular ou não: 
    bool jumpAllowed;

    //Variáveis do Input novo de pular e andar
    InputAction acaoMover, acaoPular;


    void Start()
    {
        //Visando em não ter que arrastar no inspector, pegamos o componente dele direto no Start
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        //Mapeando e chamando as ações de mover e pular do Input novo
        acaoMover = InputSystem.actions?.FindAction("Move");
        acaoPular = InputSystem.actions?.FindAction("Jump");

        acaoMover?.Enable();
        acaoPular?.Enable();

    }


    void Update()
    {
        //Criando variável de direção que lê a ação com Vector2.x
        float direcao = acaoMover.ReadValue<Vector2>().x;
        //No RigidBody LinerVelocityX (Eixo x) recebe direção vezes a velocidade
        rb.linearVelocityX = direcao * velocidade;

        //Condição com a direção que identifica se a direção é menor que zero, logo muda o sprite para direita ou esquerda de acordo com o que o jogador clicar
        if (direcao != 0)
        {
            sprite.flipX = direcao < 0;
        }


        //Identifica se o eixo y for 0 a booelana de pular fica true ou false
        if (rb.linearVelocityY == 0)
        {
            jumpAllowed = true; //Booleana true
        }
        else
        {
            jumpAllowed = false; //Booleana false
        }

        //Criamos booleana que verifica se ação de pulo for diferente de nulo e foi clicado naquele frame
        bool jumpIsPressed = acaoPular != null && acaoPular.WasPressedThisFrame();

        //Se a booleana da ação e a booleana do pulo forem verdadeiras, o RigidBody LinearY(eixo y) recebe a velocidade do pulo.
        if(jumpIsPressed && jumpAllowed)
        {
            rb.linearVelocityY = velpulo;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Se o personagem colidir com o objeto(com boxcollider) que tem a tag Espinhos, a cena reinicia
        if (collision.gameObject.CompareTag("Espinhos"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Se o personagem colidir com o objeto(com boxcollider com trigger marcado) que tem a tag Bandeira, a cena reinicia.
        if (collision.CompareTag("Bandeira"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    

}
