using System.Collections.Generic;
using UnityEngine;
using TMPro; // Wajib di-import untuk TextMeshPro

public class DropdownController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown myDropdown;

    void Start()
    {
        SetupDropdown();
    }

    void SetupDropdown()
    {
        List<string> pilihanMenu = new List<string>()
        {
            "Pilih Kelas",
            "Kelas 1 A",
            "Kelas 1 B",
            "Kelas 1 C",
            "Kelas 2 A",
            "Kelas 2 B",
            "Kelas 2 C",
            "Kelas 3 A",
            "Kelas 3 B",
            "Kelas 3 C",
            "Kelas 4 A",
            "Kelas 4 B",
            "Kelas 4 B",
            "Kelas 4 C",
            "Kelas 5 A",
            "Kelas 5 B",
            "Kelas 5 C",
            "Kelas 6 A",
            "Kelas 6 B",
            "Kelas 6 C"
        };
        myDropdown.ClearOptions();
        myDropdown.AddOptions(pilihanMenu);
        myDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }


    private void OnDropdownValueChanged(int index)
    {
        Debug.Log("Pilihan diganti ke urutan: " + index);
        string teksPilihan = myDropdown.options[index].text;
        Debug.Log("Teks yang dipilih: " + teksPilihan);
        if (index == 1)
        {
            Debug.Log("Game dimulai dengan Mode Mudah!");
        }
    }
}