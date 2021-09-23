using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private TitlesList _titlesList = new TitlesList();
    [SerializeField] private GameObject _spriteForMap;
    [SerializeField] private Sprite[] _spritesMap;

    [HideInInspector] public float minVerticalMapPosition;
    [HideInInspector] public float maxVerticalMapPosition;
    [HideInInspector] public float minHorizontalMapPosition;
    [HideInInspector] public float maxHorizontalMapPosition;

    private void Awake()
    {
        TextAsset asset = Resources.Load("testing_views_settings_hard_level") as TextAsset;
        if(asset != null)
        {
            _titlesList = JsonUtility.FromJson<TitlesList>(asset.text);
            foreach (Titles titles in _titlesList.Titles)
            {
                
                Vector3 pos = new Vector3(titles.X, titles.Y, 0);
                GameObject obj = Instantiate(_spriteForMap, pos, Quaternion.identity,gameObject.transform);
                foreach (Sprite sprite in _spritesMap)
                {
                    if(sprite.name == titles.Id)
                    {
                        obj.GetComponent<SpriteRenderer>().sprite = sprite;
                        if(sprite.rect.width != sprite.rect.height)
                        {
                            titles.X -= (sprite.rect.height - sprite.rect.width) / 200;
                            obj.transform.position = new Vector3(titles.X, titles.Y, 0);
                            obj.GetComponent<BoxCollider2D>().size = new Vector2(sprite.rect.width / 100, obj.GetComponent<BoxCollider2D>().size.y);
                        }

                        if (maxHorizontalMapPosition <= titles.X)
                        {
                            maxHorizontalMapPosition = titles.X + sprite.rect.width/200;
                        }
                        if(minHorizontalMapPosition >= titles.X)
                        {
                            minHorizontalMapPosition = titles.X - sprite.rect.width / 200;
                        }
                        if (minVerticalMapPosition >= titles.Y)
                        {
                            minVerticalMapPosition = titles.Y-sprite.rect.width/200;
                        }
                        if (maxVerticalMapPosition <= titles.Y)
                        {
                            maxVerticalMapPosition = titles.Y + sprite.rect.width / 200;
                        }

                    }

                }
               

            }
        }
        else
        {
            print("Asset is null");
        }
        
    }

    

    
}
