using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerHandler : MonoBehaviour 
{
	private void Start() 
	{
		Inventory.OnItemAction.AddListener(OnInventoryItemAction);
	}
	
	private void OnDestroy() 
	{
		Inventory.OnItemAction.RemoveListener(OnInventoryItemAction);
	}

	private void OnInventoryItemAction(int itemId, InventoryAction action)
    {
		Debug.Log($"PostRequest > ItemId {itemId}, InventoryAction {action}");
        StartCoroutine(PostRequest(itemId, action));
    }

    IEnumerator PostRequest(int itemId, InventoryAction action)
    {
        string url = "https://dev3r02.elysium.today/inventory/status";
        string authKey = "BMeHG5xqJeB4qCjpuJCTQLsqNGaqkfB6";
        WWWForm form = new WWWForm();
        form.AddField("Index", itemId);
        form.AddField("Action", (int)action);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        uwr.SetRequestHeader("Authorization", authKey);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }
}
