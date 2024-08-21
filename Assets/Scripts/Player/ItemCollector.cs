using UnityEngine;

public class ItemCollector
{      
    public void CollectItem(Vector2 pos, float attractionRange, float attractionSpeed, int itemLayer)
    {
        // �÷��̾� �ֺ��� ������ Ž��
        Collider2D[] items = Physics2D.OverlapCircleAll(pos, attractionRange, itemLayer);
        foreach (Collider2D itemCollider in items)
        {
            GameObject item = itemCollider.gameObject;
            float distanceToItem = Vector3.Distance(pos, item.transform.position);

            // �ڼ�ȿ��
            item.transform.position = Vector3.MoveTowards(item.transform.position, pos, attractionSpeed * Time.deltaTime);
        }
    }
}
