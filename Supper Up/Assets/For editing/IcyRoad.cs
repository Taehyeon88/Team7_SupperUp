using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyRoad : MonoBehaviour
{
    void Start()
    {
        // 빙판길의 물리적 마찰 설정
        CreateIceSurface();
    }

    void CreateIceSurface()
    {
        // Physic Material 생성
        PhysicMaterial iceMaterial = new PhysicMaterial();
        iceMaterial.name = "IceMaterial";
        iceMaterial.dynamicFriction = 0.1f; // 동적 마찰 계수
        iceMaterial.staticFriction = 0.1f; // 정적 마찰 계수
        iceMaterial.frictionCombine = PhysicMaterialCombine.Minimum; // 마찰 결합 방식 설정

        // 이 오브젝트에 BoxCollider 추가 및 물리 재질 설정
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.material = iceMaterial;

        // 빙판길 시각적 요소 (옵션)
        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find("Standard")); // 표준 쉐이더 사용
        renderer.material.color = new Color(0.8f, 0.9f, 1f); // 살짝 파란색
    }
}
