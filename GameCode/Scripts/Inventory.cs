using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Image image;
    public float scale = 100;
    private List<Image> images = new List<Image>();
    private List<string> items = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Sprite spr, string str)
    {
        if (items.Contains(str))
        {
            /// make it show how many of item you have \\\
            return;
        }
        else
        {
            items.Add(str);
            Image n = Instantiate(image, gameObject.transform);
            //n.rectTransform.sizeDelta = new Vector2(scale, scale);
            n.sprite = spr;
            images.Add(n);
            if (images.Count % 8 == 0)
            {
                scale = scale * .8f;
            }

            int count = -images.Count / 2;
            for (int i = 0; i < images.Count; i++)
            {
                images[i].rectTransform.sizeDelta = new Vector2(scale, scale);
                if (images.Count % 2 == 1)
                {
                    if (i < images.Count / 2)
                    {
                        images[i].rectTransform.localPosition = new Vector3((images[i].rectTransform.rect.width) * count, 0);
                    }
                    else if (i == images.Count / 2)
                    {
                        images[i].rectTransform.localPosition = new Vector3(0, 0);
                    }
                    else
                    {
                        images[i].rectTransform.localPosition = new Vector3((images[i].rectTransform.rect.width) * count, 0);
                    }
                    count++;
                }
                else
                {
                    images[i].rectTransform.localPosition = new Vector3((images[i].rectTransform.rect.width) * count, 0);
                    count++;
                }
                //n.rectTransform.sizeDelta = new Vector2(100, 100);
            }
        }
    }
}
