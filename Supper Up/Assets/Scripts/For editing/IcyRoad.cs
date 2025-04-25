using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyRoad : MonoBehaviour
{
    void Start()
    {
        // ���Ǳ��� ������ ���� ����
        CreateIceSurface();
    }

    void CreateIceSurface()
    {
        // Physic Material ����
        PhysicMaterial iceMaterial = new PhysicMaterial();
        iceMaterial.name = "IceMaterial";
        iceMaterial.dynamicFriction = 0.1f; // ���� ���� ���
        iceMaterial.staticFriction = 0.1f; // ���� ���� ���
        iceMaterial.frictionCombine = PhysicMaterialCombine.Minimum; // ���� ���� ��� ����

        // �� ������Ʈ�� BoxCollider �߰� �� ���� ���� ����
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.material = iceMaterial;

        // ���Ǳ� �ð��� ��� (�ɼ�)
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard")); // ǥ�� ���̴� ���
        renderer.material.color = new Color(0.8f, 0.9f, 1f); // ��¦ �Ķ���
    }
}
