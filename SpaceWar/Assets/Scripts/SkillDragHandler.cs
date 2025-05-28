using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Bu script, skill butonlar�n�n s�r�klenip b�rak�labilmesini sa�lar.
/// Skill sat�n al�nd�ysa (isUnlocked = true) kullan�c� bunu drag-drop ile slotlara koyabilir.
/// </summary>
public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int skillIndex; // Skill'e �zel ID (Inspector�dan ayarlanmal�)
    public Text priceText; // Inspector'dan atanacak
    public bool isUnlocked = false; // Bu skill sat�n al�nd� m�?

    private CanvasGroup canvasGroup;         // UI ��esinin raycast kontrol� i�in
    private RectTransform rectTransform;     // UI ��esinin konumunu kontrol etmek i�in
    private Transform originalParent;        // Drag i�lemi s�ras�nda geri d�nebilece�i orijinal parent
    private GameObject draggingIcon;         // S�r�klenen skill'in ekranda olu�an ge�ici kopyas�

  

    void Awake()
    {
        // Gerekli bile�enleri ba�ta al�yoruz
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }
  void Start()
    {
        // KAYITLI VER�Y� KONTROL ET
        if (PlayerPrefs.GetInt("SkillUnlocked_" + skillIndex, 0) == 1)
        {
            UnlockSkill();
        }
    }
    public void OnBeginDrag(PointerEventData eventData) // Drag ba�lad���nda �al���r. Skill kopyas�n� olu�turur ve s�r�klenmeye haz�r hale getirir.
    {
        if (!isUnlocked) return; // Skill kilitliyse i�lem yapma

        draggingIcon = Instantiate(gameObject, transform.root);         // Skill butonunun bir kopyas�n� olu�turuyoruz, b�ylece orijinal yerinde kal�r
        draggingIcon.transform.SetAsLastSibling(); 

        var group = draggingIcon.GetComponent<CanvasGroup>();        // Kopyan�n �zerinden raycast ge�mesin, yani ba�ka UI ��eleriyle �ak��mas�n
        if (group != null)
            group.blocksRaycasts = false;

        originalParent = transform.parent;        // Orijinal parent'� hat�rl�yoruz (geri d�nd�rmek gerekirse diye)
    }

    public void OnDrag(PointerEventData eventData)  // Drag devam ederken �al���r. Kopya skill UI eleman�n� fare konumuna ta��r.
    {
        if (!isUnlocked || draggingIcon == null) return;
        draggingIcon.GetComponent<RectTransform>().position = eventData.position;       // Kopya butonu farenin oldu�u yere getiriyoruz
    }


    public void OnEndDrag(PointerEventData eventData)       // Drag b�rak�ld���nda �al���r. B�rak�lan alan bir drop alan� de�ilse kopyay� yok eder.
    {
        if (!isUnlocked || draggingIcon == null) return;   
        var group = draggingIcon.GetComponent<CanvasGroup>();       // Art�k raycast'e a��k olabilir
        if (group != null)
            group.blocksRaycasts = true;

        Destroy(draggingIcon, 0.1f);         // E�er ge�erli bir drop alan�na b�rak�lmazsa kopyay� sil
    }

    public void UnlockSkill()    // Skill sat�n al�nd���nda �a�r�l�r. Skill art�k kullan�labilir (s�r�klenebilir) olur.
    {
        Debug.Log("Skill " + skillIndex + " unlocked!");
        isUnlocked = true; // Art�k drag yap�labilir   
        GetComponent<Button>().interactable = false;        // Art�k butona t�klanmas�n, ��nk� i�levi sadece drag olacak
        GetComponent<Image>().color = Color.white;        // Rengini beyaz yap, �rne�in sat�n al�nm�� hissi vermek i�in
    }
}
