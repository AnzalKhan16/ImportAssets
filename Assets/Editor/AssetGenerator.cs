using UnityEngine;
using UnityEditor;

public class AssetGenerator : EditorWindow
{
    [MenuItem("Tools/Generate Realistic Road Scene")]
    public static void GenerateScene()
    {
        // 1. Create Road
        GameObject roadGroup = new GameObject("Road System");
        
        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);
        road.name = "Road";
        road.transform.parent = roadGroup.transform;
        road.transform.localScale = new Vector3(8, 0.1f, 50);
        road.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(new Color(0.2f, 0.2f, 0.2f), "RoadMat");

        // Sidewalks
        GameObject leftSidewalk = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftSidewalk.name = "LeftSidewalk";
        leftSidewalk.transform.parent = roadGroup.transform;
        leftSidewalk.transform.localPosition = new Vector3(-4.5f, 0.05f, 0);
        leftSidewalk.transform.localScale = new Vector3(1, 0.2f, 50);
        leftSidewalk.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(new Color(0.6f, 0.6f, 0.6f), "SidewalkMat");

        GameObject rightSidewalk = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightSidewalk.name = "RightSidewalk";
        rightSidewalk.transform.parent = roadGroup.transform;
        rightSidewalk.transform.localPosition = new Vector3(4.5f, 0.05f, 0);
        rightSidewalk.transform.localScale = new Vector3(1, 0.2f, 50);
        rightSidewalk.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(new Color(0.6f, 0.6f, 0.6f), "SidewalkMat");

        // Center line
        for (int i = 0; i < 12; i++)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
            line.name = "CenterLine";
            line.transform.parent = roadGroup.transform;
            line.transform.localPosition = new Vector3(0, 0.06f, -22 + (i * 4));
            line.transform.localScale = new Vector3(0.15f, 0.01f, 2f);
            line.GetComponent<Renderer>().sharedMaterial = CreateColorMaterial(Color.white, "LineMat");
        }

        // 2. Create Street Lights along the road
        for (int i = 0; i < 5; i++)
        {
            float zPos = -20 + (i * 10);
            CreateStreetLight(new Vector3(4.5f, 0.15f, zPos));
            CreateStreetLight(new Vector3(-4.5f, 0.15f, zPos));
        }

        // 3. Create Cow (Realistic Brown)
        CreateCow(new Vector3(2, 0.1f, -5));

        // 4. Create Dog (Golden Retriever)
        CreateDog(new Vector3(-1, 0.1f, 0));

        // 5. Create Cat (Gray Tabby)
        CreateCat(new Vector3(0, 0.1f, 5));
        
        Debug.Log("Realistic road scene with procedurally generated animals and street lights generated successfully!");
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
        // Reduce smoothness for a more matte, realistic look
        if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.1f);
        if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", 0.1f);
        return mat;
    }

    private static void CreateStreetLight(Vector3 position)
    {
        GameObject lightGroup = new GameObject("StreetLight");
        lightGroup.transform.position = position;

        Material metalMat = CreateColorMaterial(new Color(0.3f, 0.3f, 0.3f), "MetalMat");
        if (metalMat.HasProperty("_Metallic")) metalMat.SetFloat("_Metallic", 0.8f);
        if (metalMat.HasProperty("_Smoothness")) metalMat.SetFloat("_Smoothness", 0.6f);

        // Base
        GameObject poleBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        poleBase.transform.parent = lightGroup.transform;
        poleBase.transform.localPosition = new Vector3(0, 0.5f, 0);
        poleBase.transform.localScale = new Vector3(0.4f, 0.5f, 0.4f);
        poleBase.GetComponent<Renderer>().sharedMaterial = metalMat;

        // Pole
        GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole.transform.parent = lightGroup.transform;
        pole.transform.localPosition = new Vector3(0, 3, 0);
        pole.transform.localScale = new Vector3(0.15f, 3f, 0.15f);
        pole.GetComponent<Renderer>().sharedMaterial = metalMat;

        // Arm
        GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        arm.transform.parent = lightGroup.transform;
        arm.transform.localPosition = new Vector3(-0.5f, 5.8f, 0);
        arm.transform.localRotation = Quaternion.Euler(0, 0, 90);
        arm.transform.localScale = new Vector3(0.1f, 0.6f, 0.1f);
        arm.GetComponent<Renderer>().sharedMaterial = metalMat;

        // Bulb Housing
        GameObject housing = GameObject.CreatePrimitive(PrimitiveType.Cube);
        housing.transform.parent = lightGroup.transform;
        housing.transform.localPosition = new Vector3(-1.2f, 5.8f, 0);
        housing.transform.localScale = new Vector3(0.6f, 0.15f, 0.4f);
        housing.GetComponent<Renderer>().sharedMaterial = metalMat;

        // Bulb
        GameObject bulb = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bulb.transform.parent = lightGroup.transform;
        bulb.transform.localPosition = new Vector3(-1.2f, 5.75f, 0);
        bulb.transform.localScale = new Vector3(0.4f, 0.1f, 0.3f);
        
        Material bulbMat = CreateColorMaterial(new Color(1f, 0.9f, 0.7f), "BulbMat");
        bulbMat.EnableKeyword("_EMISSION");
        bulbMat.SetColor("_EmissionColor", new Color(1f, 0.9f, 0.7f) * 2f);
        bulb.GetComponent<Renderer>().sharedMaterial = bulbMat;

        Light pointLight = bulb.AddComponent<Light>();
        pointLight.type = LightType.Spot;
        pointLight.color = new Color(1f, 0.9f, 0.7f);
        pointLight.range = 15;
        pointLight.intensity = 3;
        pointLight.spotAngle = 120;
        pointLight.transform.localRotation = Quaternion.Euler(90, 0, 0);
    }

    private static void CreateCow(Vector3 position)
    {
        GameObject cowGroup = new GameObject("Cow");
        cowGroup.transform.position = position;

        Material cowMat = CreateColorMaterial(new Color(0.4f, 0.25f, 0.15f), "CowMat"); // Brown cow
        Material spotMat = CreateColorMaterial(Color.white, "CowSpotMat"); 
        Material hornMat = CreateColorMaterial(new Color(0.9f, 0.9f, 0.8f), "HornMat"); 
        Material udderMat = CreateColorMaterial(new Color(0.9f, 0.7f, 0.7f), "UdderMat"); 

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = cowGroup.transform;
        body.transform.localPosition = new Vector3(0, 1.2f, 0);
        body.transform.localScale = new Vector3(1.2f, 0.9f, 2.6f);
        body.GetComponent<Renderer>().sharedMaterial = cowMat;

        // Spot on body
        GameObject spot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spot.transform.parent = cowGroup.transform;
        spot.transform.localPosition = new Vector3(0, 1.25f, 0.3f);
        spot.transform.localScale = new Vector3(1.22f, 0.82f, 1.0f);
        spot.GetComponent<Renderer>().sharedMaterial = spotMat;

        // Udder
        GameObject udder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        udder.transform.parent = cowGroup.transform;
        udder.transform.localPosition = new Vector3(0, 0.7f, -0.4f);
        udder.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
        udder.GetComponent<Renderer>().sharedMaterial = udderMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.parent = cowGroup.transform;
        head.transform.localPosition = new Vector3(0, 1.6f, 1.4f);
        head.transform.localScale = new Vector3(0.7f, 0.7f, 0.8f);
        head.GetComponent<Renderer>().sharedMaterial = spotMat;

        // Snout
        GameObject snout = GameObject.CreatePrimitive(PrimitiveType.Cube);
        snout.transform.parent = cowGroup.transform;
        snout.transform.localPosition = new Vector3(0, 1.45f, 1.8f);
        snout.transform.localScale = new Vector3(0.6f, 0.4f, 0.4f);
        snout.GetComponent<Renderer>().sharedMaterial = udderMat;

        // Horns
        GameObject leftHorn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        leftHorn.transform.parent = cowGroup.transform;
        leftHorn.transform.localPosition = new Vector3(-0.4f, 1.9f, 1.3f);
        leftHorn.transform.localRotation = Quaternion.Euler(0, 0, 45);
        leftHorn.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
        leftHorn.GetComponent<Renderer>().sharedMaterial = hornMat;

        GameObject rightHorn = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        rightHorn.transform.parent = cowGroup.transform;
        rightHorn.transform.localPosition = new Vector3(0.4f, 1.9f, 1.3f);
        rightHorn.transform.localRotation = Quaternion.Euler(0, 0, -45);
        rightHorn.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
        rightHorn.GetComponent<Renderer>().sharedMaterial = hornMat;

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
            leg.transform.localScale = new Vector3(0.25f, 0.6f, 0.25f);
            leg.GetComponent<Renderer>().sharedMaterial = cowMat;
        }

        // Tail
        GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tail.transform.parent = cowGroup.transform;
        tail.transform.localPosition = new Vector3(0, 1.0f, -1.4f);
        tail.transform.localRotation = Quaternion.Euler(30, 0, 0);
        tail.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        tail.GetComponent<Renderer>().sharedMaterial = cowMat;
    }

    private static void CreateDog(Vector3 position)
    {
        GameObject dogGroup = new GameObject("Dog");
        dogGroup.transform.position = position;

        Material dogMat = CreateColorMaterial(new Color(0.8f, 0.6f, 0.2f), "DogMat"); // Golden Retriever
        Material darkMat = CreateColorMaterial(new Color(0.2f, 0.2f, 0.2f), "DogNoseMat");

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = dogGroup.transform;
        body.transform.localPosition = new Vector3(0, 0.8f, 0);
        body.transform.localScale = new Vector3(0.5f, 0.5f, 1.1f);
        body.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.parent = dogGroup.transform;
        head.transform.localPosition = new Vector3(0, 1.1f, 0.6f);
        head.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        head.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Snout
        GameObject snout = GameObject.CreatePrimitive(PrimitiveType.Cube);
        snout.transform.parent = dogGroup.transform;
        snout.transform.localPosition = new Vector3(0, 1.0f, 0.85f);
        snout.transform.localScale = new Vector3(0.25f, 0.25f, 0.3f);
        snout.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Nose
        GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        nose.transform.parent = dogGroup.transform;
        nose.transform.localPosition = new Vector3(0, 1.05f, 1.02f);
        nose.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        nose.GetComponent<Renderer>().sharedMaterial = darkMat;

        // Ears
        GameObject leftEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftEar.transform.parent = dogGroup.transform;
        leftEar.transform.localPosition = new Vector3(-0.25f, 1.1f, 0.5f);
        leftEar.transform.localScale = new Vector3(0.1f, 0.3f, 0.2f);
        leftEar.GetComponent<Renderer>().sharedMaterial = dogMat;

        GameObject rightEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightEar.transform.parent = dogGroup.transform;
        rightEar.transform.localPosition = new Vector3(0.25f, 1.1f, 0.5f);
        rightEar.transform.localScale = new Vector3(0.1f, 0.3f, 0.2f);
        rightEar.GetComponent<Renderer>().sharedMaterial = dogMat;

        // Legs
        Vector3[] legPositions = {
            new Vector3(0.15f, 0.4f, 0.4f), new Vector3(-0.15f, 0.4f, 0.4f),
            new Vector3(0.15f, 0.4f, -0.4f), new Vector3(-0.15f, 0.4f, -0.4f)
        };
        foreach (var pos in legPositions)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.parent = dogGroup.transform;
            leg.transform.localPosition = pos;
            leg.transform.localScale = new Vector3(0.12f, 0.4f, 0.12f);
            leg.GetComponent<Renderer>().sharedMaterial = dogMat;
        }

        // Tail
        GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tail.transform.parent = dogGroup.transform;
        tail.transform.localPosition = new Vector3(0, 0.9f, -0.6f);
        tail.transform.localRotation = Quaternion.Euler(60, 0, 0);
        tail.transform.localScale = new Vector3(0.08f, 0.3f, 0.08f);
        tail.GetComponent<Renderer>().sharedMaterial = dogMat;
    }

    private static void CreateCat(Vector3 position)
    {
        GameObject catGroup = new GameObject("Cat");
        catGroup.transform.position = position;

        Material catMat = CreateColorMaterial(new Color(0.5f, 0.5f, 0.5f), "CatMat"); // Gray cat

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.parent = catGroup.transform;
        body.transform.localPosition = new Vector3(0, 0.4f, 0);
        body.transform.localScale = new Vector3(0.25f, 0.3f, 0.6f);
        body.GetComponent<Renderer>().sharedMaterial = catMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.transform.parent = catGroup.transform;
        head.transform.localPosition = new Vector3(0, 0.6f, 0.35f);
        head.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        head.GetComponent<Renderer>().sharedMaterial = catMat;

        // Ears
        GameObject leftEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftEar.transform.parent = catGroup.transform;
        leftEar.transform.localPosition = new Vector3(-0.1f, 0.75f, 0.35f);
        leftEar.transform.localRotation = Quaternion.Euler(0, 0, 30);
        leftEar.transform.localScale = new Vector3(0.1f, 0.15f, 0.1f);
        leftEar.GetComponent<Renderer>().sharedMaterial = catMat;

        GameObject rightEar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightEar.transform.parent = catGroup.transform;
        rightEar.transform.localPosition = new Vector3(0.1f, 0.75f, 0.35f);
        rightEar.transform.localRotation = Quaternion.Euler(0, 0, -30);
        rightEar.transform.localScale = new Vector3(0.1f, 0.15f, 0.1f);
        rightEar.GetComponent<Renderer>().sharedMaterial = catMat;

        // Legs
        Vector3[] legPositions = {
            new Vector3(0.1f, 0.2f, 0.2f), new Vector3(-0.1f, 0.2f, 0.2f),
            new Vector3(0.1f, 0.2f, -0.2f), new Vector3(-0.1f, 0.2f, -0.2f)
        };
        foreach (var pos in legPositions)
        {
            GameObject leg = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            leg.transform.parent = catGroup.transform;
            leg.transform.localPosition = pos;
            leg.transform.localScale = new Vector3(0.06f, 0.2f, 0.06f);
            leg.GetComponent<Renderer>().sharedMaterial = catMat;
        }
        
        // Tail
        GameObject tail = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tail.transform.parent = catGroup.transform;
        tail.transform.localPosition = new Vector3(0, 0.5f, -0.4f);
        tail.transform.localRotation = Quaternion.Euler(30, 0, 0);
        tail.transform.localScale = new Vector3(0.04f, 0.3f, 0.04f);
        tail.GetComponent<Renderer>().sharedMaterial = catMat;
    }
}
