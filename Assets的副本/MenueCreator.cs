using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonInternal 
{
    public string ButtonName = "Unknow";

    public Action ButtonClickAction = null;

    public GameObject ThisButton;

    public List<ButtonInternal> SubButtonList = new List<ButtonInternal>();


    public ButtonInternal(string buttonName,GameObject thisBtn)
    {
        ButtonName = buttonName;
        ThisButton = thisBtn;
        ThisButton.name = buttonName;

        ButtonClickAction = () => { Debug.Log(" button " + buttonName + " clicked ..."); };
    }
}


public class MenueCreator : MonoBehaviour
{
    public GameObject Root;

    public GameObject Root2;

    public Font DefaultFont;

    public Color DefaultColor=Color .black ;

    public float DefaultButtonWidth=160.0f;      public float DefaultButtonHeight=30.0f;

    public bool bButtonShrinkAsTextWidth = true;

    public bool TriggerWithClick=false ; 
    public Sprite  NormalButtonTexture;

    public Sprite  SelectButtonTexture;

    public Sprite BackgroundTexture;

    public int ShrinkPading=0;
   
    void Start()
    {
        Root.tag = "Root";
       
        Root.AddComponent<MouseEvent>();
       
        ButtonInternal rootBtn = new ButtonInternal("Root", Root);
        List<String> SubBtnName1 = new List<String > { "RootSub1", "RootSub2", "RootSub3", "RootSub4" };
        GetTestButtonInternal(rootBtn,SubBtnName1 );
        List<String> SubBtnName2 = new List<String> { "sub5", "sub6", "sub7" };
        GetTestButtonInternal(rootBtn.SubButtonList[0],SubBtnName2 );

        ButtonInternal rootBtn2 = new ButtonInternal("Root2", Root2);

        FindFather(rootBtn ,rootBtn2);



        Restart();



    }


    private ButtonInternal GetTestButtonInternal(ButtonInternal rootBtn,List <String >SubBtnName)     {
        ButtonInternal RawBtn = rootBtn ;
        List<String> BtnInSubMenuName = SubBtnName ;

        List<GameObject> SubMenu = CreatMenu(RawBtn.ThisButton, BtnInSubMenuName.Count, BtnInSubMenuName);

        for (int i = 0; i < BtnInSubMenuName.Count; i++)
        {
            RawBtn.SubButtonList.Add(new ButtonInternal(BtnInSubMenuName[i], SubMenu[i]));
        } 

         return RawBtn ;     }


    private void FindFather(ButtonInternal father,ButtonInternal son)
    {
        father.SubButtonList.Add(son);
        foreach (Transform child in father .ThisButton .transform ){
            if(child .tag =="Hide"){
                son.ThisButton.transform.SetParent(child);
            }
        }

    }

    private void Init(ButtonInternal buttonInternal)     {
    
       

    }



        // Update is called once per frame
        void Update()
    {

    }







    public List<GameObject> CreatMenu(GameObject rawButton, int btnNumberInMenu, List<string> btnName)
    {
        List<GameObject> subMenu = new List<GameObject>();

        float btnLength = 0;

        GameObject background = BackgroundGenerate(rawButton, rawButton.name + "Bg",BackgroundTexture,ShrinkPading);
        background.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30.0f * btnNumberInMenu + 0.0f * (btnNumberInMenu - 1));
        background.GetComponent<VerticalLayoutGroup>().spacing = 1;

        for (int i = 0; i < btnNumberInMenu; i++)
        {

            //循环生成Button
            GameObject btnInMenu = ButtonGenerate(background, background.name + "btn" + i,NormalButtonTexture ,SelectButtonTexture );
            btnInMenu.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            btnInMenu.GetComponent<RectTransform>().localScale = rawButton.GetComponent<RectTransform>().localScale;
        

            //循环生成Text
            GameObject texUnderBtn = TextGenerate(btnInMenu, btnName [i], DefaultFont, Color .black );
            texUnderBtn.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            texUnderBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);

            if (texUnderBtn.GetComponent<Text>().preferredWidth > btnLength)
                btnLength = texUnderBtn.GetComponent<Text>().preferredWidth;

            subMenu.Add(btnInMenu);

        }
     
        background.GetComponent<RectTransform>().sizeDelta = GetButtonSize(btnLength, btnNumberInMenu);

        background.tag = "Hide";





        return subMenu;
    }

    public void Restart()
    {
        List<GameObject> btnNex = new List<GameObject>();
        while (GameObject.FindWithTag("Hide"))
        {
            btnNex.Add(GameObject.FindWithTag("Hide"));
            GameObject.FindWithTag("Hide").tag = "Normal";
        }

        foreach (GameObject obj in btnNex)
        {
            obj.SetActive(false);
            obj.tag = "Hide";
        }
    }


    public GameObject ButtonGenerate(GameObject btnParent, string btnName,Sprite  normaltexture,Sprite  selecttexture)
    {
        GameObject btn = new GameObject(btnName);
        btn.AddComponent<RectTransform>();
        btn.AddComponent<Button>();
        btn.AddComponent<Image>();
        btn.AddComponent<MouseEvent>();

        btn.transform.SetParent(btnParent.transform, false);
        btn.name = btnName;
        btn.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        btn.GetComponent <Button >().targetGraphic  = btn .GetComponent <Image >();
        btn.GetComponent<Image>().sprite = normaltexture;
        SpriteState btnImage = new SpriteState();
        btnImage.disabledSprite = normaltexture;
        btnImage.highlightedSprite = selecttexture;
        btnImage.pressedSprite = normaltexture;
        btn.GetComponent<Button>().spriteState = btnImage;



        return btn;

    }
    public GameObject TextGenerate(GameObject textParent, String texName, Font texFont, Color texColor)
    {
        GameObject tex = new GameObject(texName);
        tex.AddComponent<Text>();
        tex.AddComponent<RectTransform>();
        tex.AddComponent<ContentSizeFitter>();

        tex.transform.SetParent(textParent.transform, false);
        tex.GetComponent<Text>().font = texFont;
        tex.GetComponent<Text>().color = texColor;
        tex.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        tex.GetComponent<Text>().text = texName;
        tex.name = texName;


        return tex;
    }
    public GameObject BackgroundGenerate(GameObject bgParent, string bgName,Sprite bgImage,int shrinkpading)
    {
        GameObject bg = new GameObject(bgName);
        bg.AddComponent<RectTransform>();
        bg.AddComponent<Image>();
        bg.AddComponent<VerticalLayoutGroup>();
        //if(bButtonShrinkAsTextWidth == true){
        //    bg.GetComponent<VerticalLayoutGroup>().padding.bottom = shrinkpading;
        //    bg.GetComponent<VerticalLayoutGroup>().padding.left  = shrinkpading;
        //    bg.GetComponent<VerticalLayoutGroup>().padding.right  = shrinkpading;
        //    bg.GetComponent<VerticalLayoutGroup>().padding.top  = shrinkpading;
        //}


        bg.transform.SetParent(bgParent.transform, false);

        bg.GetComponent<RectTransform>().anchorMax = new Vector2(1.0f, 1.0f);
        bg.GetComponent<RectTransform>().anchorMin  = new Vector2(1.0f, 1.0f);
        bg.GetComponent<RectTransform>().pivot  = new Vector2(0f, 1.0f);

        bg.GetComponent<Image>().sprite = bgImage;
        return bg;
    }

    public Vector2 GetButtonSize(float btnlen,int btnnum)
    {

        if (bButtonShrinkAsTextWidth == true)
            return new Vector2(btnlen +ShrinkPading +2, 30.0f * btnnum  + 0.0f * (btnnum  - 1)+ShrinkPading *2);
        else
            return  new Vector2(DefaultButtonWidth, DefaultButtonHeight * btnnum);
    }

}
