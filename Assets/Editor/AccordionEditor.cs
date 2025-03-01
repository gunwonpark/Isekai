#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class AccordionEditor
{
    [MenuItem("GameObject/UI/Accordion", false, 10)]
    public static void CreateAccordion()
    {
        Canvas root = GameObject.FindObjectOfType<Canvas>();
        if(root == null)
        {
            GameObject canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            root = canvas.GetComponent<Canvas>();
        }

        GameObject panel = new GameObject("AccordionPanel", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(AccordionPanel), typeof(ContentSizeFitter));
        var layoutGroup = panel.GetComponent<VerticalLayoutGroup>();
        var contentSizeFitter = panel.GetComponent<ContentSizeFitter>();

        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;

        layoutGroup.childControlHeight = true;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        
        panel.transform.SetParent(root.gameObject.transform, false);
       

        for (int i = 0; i < 2; i++)
        {
            CreateAccordionItem(panel.transform, $"AccordionItem_{i + 1}");
        }

        Undo.RegisterCreatedObjectUndo(panel, "Create Accordion");
        Selection.activeGameObject = panel;
    }

    private static void CreateAccordionItem(Transform parent, string name)
    {
        GameObject item = new GameObject(name, typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(LayoutElement), typeof(AccordionItem));

        var layoutGroup = item.GetComponent<VerticalLayoutGroup>();
        var accordionItem = item.GetComponent<AccordionItem>();

        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childScaleHeight = true;

        // Header
        GameObject header = new GameObject("Header", typeof(RectTransform));
        header.transform.SetParent(item.transform, false);
        var headerRect = header.GetComponent<RectTransform>();
        headerRect.sizeDelta = new Vector2(0, 50); 

        GameObject hText = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        hText.transform.SetParent(header.transform, false);
        hText.GetComponent<RectTransform>().localPosition = new Vector3(108, 0, 0);


        var hContentText = hText.GetComponent<TextMeshProUGUI>();
        hContentText.text = "Header";

        GameObject hButton = new GameObject("Button", typeof(RectTransform), typeof(Button), typeof(Image));
        hButton.transform.SetParent(header.transform, false);
        hButton.GetComponent<RectTransform>().localPosition = new Vector3(-62, 0, 0);

        // Content
        GameObject content = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(LayoutElement));
        content.transform.SetParent(item.transform, false);

        var clayoutGroup = content.GetComponent<VerticalLayoutGroup>();

        clayoutGroup.childForceExpandHeight = false;
        clayoutGroup.childForceExpandWidth = false;
        clayoutGroup.childControlHeight = true;

        GameObject cText = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        cText.transform.SetParent(content.transform, false);

        var cContentText = cText.GetComponent<TextMeshProUGUI>();
        cContentText.text = "Whatever you want you can add content";

        //accordionItem
        accordionItem.SetHeader(headerRect);
        accordionItem.SetContent(content.GetComponent<RectTransform>());
        accordionItem.SetToggleButton(hButton.GetComponent<Button>());

        // 부모에 붙이기
        item.transform.SetParent(parent, false);
    }
}
#endif