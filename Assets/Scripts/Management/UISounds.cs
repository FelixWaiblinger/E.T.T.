using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private AudioClip _buttonSmall;
    [SerializeField] private AudioClip _buttonLarge;
    [SerializeField] private AudioClip _menu;
    private Transform _camera;

    void Start()
    {
        _camera = Camera.main.transform;
    }

    public void ButtonSmall()
    {
        AudioSource.PlayClipAtPoint(_buttonSmall, _camera.position, _gameData.Volume);
    }

    public void ButtonLarge()
    {
        AudioSource.PlayClipAtPoint(_buttonLarge, _camera.position, _gameData.Volume);
    }

    public void Menu()
    {
        AudioSource.PlayClipAtPoint(_menu, _camera.position, _gameData.Volume);
    }
}
