using UnityEngine;
using UnityEditor;

public class AssetGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Colorful Road Scene")]
    public static void GenerateScene()
    {
        // 1. Create Road
        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
        road.name = "Road";
        road.transform.localScale = new Vector3(10, 0.1f, 50);
        road.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(Color.gray, "RoadMat");

        // 2. Create Street Lights along the road
        for (int i = 0; i < 5; i++)
        {
            float zPos = -20 + (i * 10);
            CreateStreetLight(new Vector3(4.5f, 0, zPos));
            CreateStreetLight(new Vector3(-4.5f, 0, zPos));
        }

        // 3. Create Cow (Pinkish)
        CreateCow(new Vector3(2, 0, -5));

        // 4. Create Dog (Blue)
        CreateDog(new Vector3(-1, 0, 0));

        // 5. Create Cat (Orange)
        CreateCat(new Vector3(0, 0, 5));
        
        Debug.Log("Colorful road scene with procedurally generated animals and street lights generated successfully!");
    }

    private static Material CreateColorMaterial(Color color, string name)
    {
        // Try Lit shader first (for Universal Render Pipeline)
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) 
        {
            // Fallback to Standard shader if URP is not installed
            shader = Shader.Find("Standard");
        }
        
        Material mat = new Material(shader);
        mat.color = color;
        return mat;
    }

    private static void CreateStreetLight(Vector3 position)
    {
        GameObject lightGroup = new GameObject("StreetLight");
        lightGroup.transform.position = position;

        GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole.transform.parent = lightGroup.transform;
        pole.transform.localPosition = new Vector3(0, 2, 0);
        pole.transform.localScale = new Vector3(0.2f, 2f, 0.2f);
        pole.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(Color.grey, "PoleMat");

        GameObject bulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bulb.transform.parent = lightGroup.transform;
        bulb.transform.localPosition = new Vector3(0, 4.2f, 0);
        bulb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        Material bulbMat = CreateColorMaterial(Color.yellow, "BulbMat");
        bulbMat.EnableKeyword("_EMISSION");
        bulbMat.SetColor("_EmissionColor", Color.yellow);
        bulb.GetComponent<Renderer>().sharedMaterial = bulbMat;

        Light pointLight = bulb.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.color = Color.yellow;
        pointLight.range = 10;
        pointLight.intensity = 2;
    }

    private static void CreateCow(Vector3 position)
    {
        GameObject cowGroup = new GameObject("Cow");
        cowGroup.transform.position = position;

        Material cowMat = CreateColorMaterial(new Color(1f, 0.5f, 0.8f), "CowMat"); // Pinkish cow

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = cowGroup.transform;
        body.transform.localPosition = new Vector3(0, 1.2f, 0);
        body.transform.localScale = new Vector3(1.2f, 0.8f, 2.5f);
        body.GetComponent<Renderer>().sharedMaterial = cowMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.parent = cowGroup.transform;
        head.transform.localPosition = new Vector3(0, 1.6f, 1.4f);
        head.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        head.GetComponent<Renderer>().sharedMaterial = cowMat;

        // Legs
        Vector3[] legPositions = {
            new Vector3(0.5f, 0.6f, 1f), new Vector3(-0.5f, 0.6f, 1f),
            new Vector3(0.5f, 0.6f, -1f), new Vector3(-0.5f, 0.6f, -1f)
        };
        foreach (var pos in legPositions)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.parent = cowGroup.transform;
            leg.transform.localPosition = pos;
            leg.transform.localScale = new Vector3(0.3f, 0.6f, 0.3f);
            leg.GetComponent<Renderer>().sharedMaterial = cowMat;
        }
    }

    private static void CreateDog(Vector3 position)
    {
        GameObject dogGroup = new GameObject("Dog");
        dogGroup.transform.position = position;

        Material dogMat = CreateColorMaterial(new Color(0.2f, 0.6f, 1f), "DogMat"); // Blue dog

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = dogGroup.transform;
        body.transform.localPosition = new Vector3(0, 0.8f, 0);
        body.transform.localScale = new Vector3(0.6f, 0.5f, 1.2f);
        body.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.parent = dogGroup.transform;
        head.transform.localPosition = new Vector3(0, 1.1f, 0.7f);
        head.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        head.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Legs
        Vector3[] legPositions = {
            new Vector3(0.2f, 0.4f, 0.5f), new Vector3(-0.2f, 0.4f, 0.5f),
            new Vector3(0.2f, 0.4f, -0.5f), new Vector3(-0.2f, 0.4f, -0.5f)
        };
        foreach (var pos in legPositions)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.parent = dogGroup.transform;
            leg.transform.localPosition = pos;
            leg.transform.localScale = new Vector3(0.15f, 0.4f, 0.15f);
            leg.GetComponent<Renderer>().sharedMaterial = dogMat;
        }
    }

    private static void CreateCat(Vector3 position)
    {
        GameObject catGroup = new GameObject("Cat");
        catGroup.transform.position = position;

        Material catMat = CreateColorMaterial(new Color(1f, 0.6f, 0.2f), "CatMat"); // Orange cat

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = catGroup.transform;
        body.transform.localPosition = new Vector3(0, 0.5f, 0);
        body.transform.localScale = new Vector3(0.3f, 0.3f, 0.8f);
        body.GetComponent<Renderer>().sharedMaterial = catMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.transform.parent = catGroup.transform;
        head.transform.localPosition = new Vector3(0, 0.7f, 0.4f);
        head.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        head.GetComponent<Renderer>().sharedMaterial = catMat;

        // Legs
        Vector3[] legPositions = {
            new Vector3(0.1f, 0.25f, 0.3f), new Vector3(-0.1f, 0.25f, 0.3f),
            new Vector3(0.1f, 0.25f, -0.3f), new Vector3(-0.1f, 0.25f, -0.3f)
        };
        foreach (var pos in legPositions)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.parent = catGroup.transform;
            leg.transform.localPosition = pos;
            leg.transform.localScale = new Vector3(0.08f, 0.25f, 0.08f);
            leg.GetComponent<Renderer>().sharedMaterial = catMat;
        }
        
        // Tail
        GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tail.transform.parent = catGroup.transform;
        tail.transform.localPosition = new Vector3(0, 0.7f, -0.5f);
        tail.transform.localScale = new Vector3(0.05f, 0.3f, 0.05f);
        tail.transform.localRotation = Quaternion.Euler(45, 0, 0);
        tail.GetComponent<Renderer>().sharedMaterial = catMat;
    }
}
