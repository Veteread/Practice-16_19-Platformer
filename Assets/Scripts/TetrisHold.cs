using UnityEngine;

public class TetrisHold : MonoBehaviour
{
    RaycastHit2D hit;
    public GameObject Player;
    public GameObject Tet1;
    public GameObject Tet2;
    public Transform HoldPoint;

    private PlayerMove playerMove;
    private MaterialVars materialVars;
    private GameObject splitPartPrefab;
    private GameObject splitPartPrefab2;

    public bool Hold;
    public int maxSplitParts = 4; // Максимальное количество распилов
    public float Radius;
    public float Distance;
    public float Throw;
    public float distanceFromPlayer = 0.2f;

    private float rotate = 90;
    private int tagTet;
    private int gems;    

    private void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        materialVars = GetComponent<MaterialVars>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Hold)
            {
                switch (tagTet)
                {
                    case 1:
                        Debug.Log("Нельзя разделить, сильно мелкий камень");
                        break;
                    case 2:
                        maxSplitParts = 2;
                        CreatePrefabTet();
                        break;
                    case 3:
                        maxSplitParts = 3;
                        CreatePrefabTet();
                        break;
                    case 4:
                        maxSplitParts = 4;
                        CreatePrefabTet();
                        break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!Hold)
            {
                Physics2D.queriesStartInColliders = false;
                hit = Physics2D.CircleCast(transform.position, Radius, Vector2.right * transform.localScale.x, Distance);
                if (hit.collider != null && hit.collider.tag == "Tetris")
                {
                    matTet();
                    FreezeTetris();
                }
            }
            else
            {
                Hold = false;
                if (hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
                {
                    AnFreezeTetris();
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x, 1) * Throw;
                }
            }
        }
        if (Hold)
        {
            hit.collider.gameObject.transform.position = HoldPoint.position;
            if (HoldPoint.position.x > transform.position.x && Hold == true)
            {
                hit.collider.gameObject.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            else if (HoldPoint.position.x < transform.position.x && Hold == true)
            {
                hit.collider.gameObject.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Hold)
            {
                hit.collider.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, rotate);
                rotate += 90;
            }
        }
    }

    //public void SplitObject()
    //{
    //    // Проверяем, количество распилов
    //    if (maxSplitParts <= 0)
    //    {
    //        Debug.LogWarning("Максимальное количество распилов достигнуто.");
    //        return;
    //    }

    //    // Создаем части объекта
    //    GameObject[] splitParts = new GameObject[maxSplitParts];
    //    for (int i = 0; i < maxSplitParts; i++)
    //    {
    //        splitParts[i] = Instantiate(splitPartPrefab, Player.transform.position, Quaternion.identity);
    //    }
    //    //splitPartPrefab = Tet1;
    //    //splitPartPrefab2 = Tet2;

    //    // Вычисляем позиции для каждой части объекта
    //    Vector3 leftPosition = Player.transform.position + Vector3.left * distanceFromPlayer;
    //    Vector3 rightPosition = Player.transform.position + Vector3.right * distanceFromPlayer;

    //    // Перемещаем части объекта на позиции слева и справа от игрока
    //    for (int i = 0; i < maxSplitParts; i++)
    //    {
    //        float offset = (i % 2 == 0) ? 0f : distanceFromPlayer;
    //        Vector3 position = (i < maxSplitParts / 2) ? leftPosition : rightPosition;
    //        splitParts[i].transform.position = position + Vector3.forward * offset;
    //        //splitParts[i].transform.SetParent(Player.transform);
    //    }       

    //    // Удаляем исходный объект
    //    Destroy(hit.collider.gameObject);
    //    Hold = false;
    //}

    public void CreatePrefabTet()
    {       
        Vector3 leftPosition = Player.transform.position + Vector3.left * distanceFromPlayer;
        Vector3 rightPosition = Player.transform.position + Vector3.right * distanceFromPlayer;
        Tet1 = Instantiate(Tet1, leftPosition, Quaternion.identity);
        Tet2 = Instantiate(Tet2, rightPosition, Quaternion.identity);
        Destroy(hit.collider.gameObject);
        Hold = false;
    }

    private void matTet()
    {
        MaterialVars materialVars = hit.transform.GetComponent<MaterialVars>();
        tagTet = materialVars.Parts;
        gems = materialVars.Gems;
        Tet1 = materialVars.SplitPartPrefab;
        Tet2 = materialVars.SplitPartPrefab2;
        Hold = true;
    }

    private void FreezeTetris()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void AnFreezeTetris()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.None;
    }
}
