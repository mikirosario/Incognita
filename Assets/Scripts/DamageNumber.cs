using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
	private const float MinRange = 0.1f;
	private const float MaxRange = 10f;
	public enum Effect
	{
		SimpleRise = 0
	}
	public enum Type
	{
		Damage = 0,
		Posion,
		Healing,
	}
	[SerializeField] private TextMeshPro _damage;
	[SerializeField] private MeshRenderer _meshRenderer;
	[SerializeField, Range(MinRange, MaxRange)] private float _lifetimeInSeconds = 1f;
	[SerializeField, Range(MinRange, MaxRange)] private float _speed = 1f;
	private TextMeshPro Damage { get { return _damage; } }
	private MeshRenderer MeshRenderer { get { return _meshRenderer; } }
	public float Lifetime { get { return _lifetimeInSeconds; } set { _lifetimeInSeconds = Mathf.Clamp(value, MinRange, MaxRange); } }
	public float Speed { get { return _speed; } set { _speed = Mathf.Clamp(value, MinRange, MaxRange); } }
	public Color Color { get { return Damage.color; } set { Damage.color = value; } }

	private void Awake()
	{
		MeshRenderer.enabled = false;
	}

	private IEnumerator SimpleRiseAsync()
	{
		do
		{
			transform.position += (Vector3)Vector2.up * Speed * Time.deltaTime;
			yield return null;
			_lifetimeInSeconds -= Time.deltaTime;
		} while (Lifetime > 0f);
		Destroy(gameObject);
	}

	static public Color GetColor(Type damageType)
	{
		switch (damageType)
		{
			case Type.Damage:
				return Color.white;
			case Type.Posion:
				return Color.magenta;
			case Type.Healing:
				return Color.green;
			default:
				return Color.white;
		}
	}

	public void Display(Effect effect, uint damage)
	{
		switch (effect)
		{
			case Effect.SimpleRise:
				Damage.text = damage.ToString();
				MeshRenderer.enabled = true;
				StartCoroutine(SimpleRiseAsync());
				break;
		}
	}
}
