using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class UI_TreeConectionDetails
{
    public UI_TreeConnectionHandler childNode;
    public NodeDirectionType direction;
    [Range(100f, 350f)] public float length;
}

public class UI_TreeConnectionHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConectionDetails[] connectionDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if(connectionImage != null)
            originalColor = connectionImage.color;
    }

   public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> childrentoReturn = new List<UI_TreeNode>();

        foreach(var node in connectionDetails)
        {
            if(node.childNode != null)
                childrentoReturn.Add(node.childNode.GetComponent<UI_TreeNode>());
        }

        return childrentoReturn.ToArray();
    }

    private void UpdateConnection()
    {
        for(int i = 0; i < connectionDetails.Length; i++)
        {
            var detail = connectionDetails[i];
            var connection = connections[i];

            Vector2 targetPosition = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.DirectConnection(detail.direction, detail.length);

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPosition);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();

        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();

        foreach(var node in connectionDetails)
        {
            if(node.childNode == null)
                continue;
            node.childNode?.UpdateConnection();
        }
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : originalColor;
    }
    public void SetConnectionImage(Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
    public void OnValidate()
    {
        if (connectionDetails.Length <= 0)
            return;

        if (connectionDetails.Length != connections.Length)
        {
            Debug.Log("Amount of details should be same as connections");
            return;
        }

        UpdateConnection();
    }
}



