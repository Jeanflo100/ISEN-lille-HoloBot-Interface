﻿using UnityEngine;


/*
 * Here is the script which move the canvas with the interface, following the vision of the camera, but let a little marge to permet the user to click on button which are on sides.
 */

public class suiviCamera : MonoBehaviour
{

    public GameObject mainCam;

    // Position of the camera
    private float positionX;
    private float positionY;
    private float positionZ;

    // Distance between the object and the camera
    private float distanceX;
    private float distanceY;
    private float distanceZ;
    private float distanceEcart;
    private float distance;

    // Angles of the camera
    private float angleX;
    private float angleY;

    // Angles between the object and the camera
    private float angleObjetX;
    private float angleObjetY;

    // Ecart d'angle avant translation de l'objet
    private float angleEcartX;
    private float angleEcartY;
    private float angleMultiplicateurEcartX;
    private float angleMultiplicateurEcartY;
    private float angleMarge;

    // Size of the object
    private float width;
    private float height;
    private float scaleX;
    private float scaleY;

    
    void Start()
    {
        // Put in memory the distance between the camera and the object at the beginning to keep the object at this distance (but with others directions)
        distanceX = transform.position.x - mainCam.transform.position.x;
        distanceY = transform.position.y - mainCam.transform.position.y;
        distanceZ = transform.position.z - mainCam.transform.position.z;
        distanceEcart = 0.1f;       // Add 0.1 in order to keep the cursor in front the object
        distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY + distanceZ * distanceZ) + distanceEcart;
        // Angle of the camera to replace the object in front of it
        angleObjetX = mainCam.transform.eulerAngles.x * Mathf.PI / 180;
        angleObjetY = mainCam.transform.eulerAngles.y * Mathf.PI / 180;

        // Put the object in front of the camera
        distanceZ = distance * Mathf.Sin(angleObjetX + Mathf.PI / 2) * Mathf.Cos(angleObjetY);
        distanceX = distance * Mathf.Sin(angleObjetX + Mathf.PI / 2) * Mathf.Sin(angleObjetY);
        distanceY = distance * Mathf.Cos(angleObjetX + Mathf.PI / 2);
        transform.position = new Vector3(distanceX, distanceY, distanceZ);
        transform.rotation = Quaternion.Euler(angleObjetX * 180 / Mathf.PI, angleObjetY * 180 / Mathf.PI, 0f);


        // Initialisation des angles d'écart entre angles objets et angles caméra
        RectTransform canvas = gameObject.GetComponent<RectTransform>();
        width = canvas.rect.width;
        height = canvas.rect.height;
        scaleX = canvas.localScale.x;
        scaleY = canvas.localScale.y;
        angleEcartY = Mathf.Atan(width * scaleX / distance);
        angleEcartX = Mathf.Atan(height * scaleY / distance);
        angleMarge = 0.1f;
        angleMultiplicateurEcartY = ((Mathf.PI / 2) / angleEcartY) - angleMarge;
        angleMultiplicateurEcartX = ((Mathf.PI / 2) / angleEcartX) - angleMarge;

        // Vérification des angles d'écart
        if (angleMultiplicateurEcartY < 1 + angleMarge)
        {
            angleMultiplicateurEcartY = 1 + angleMarge;
            angleEcartY = Mathf.PI / ((angleMultiplicateurEcartY + angleMarge) * 2);
        }
        if (angleMultiplicateurEcartX < 1 + angleMarge)
        {
            angleMultiplicateurEcartX = 1 + angleMarge;
            angleEcartX = Mathf.PI / ((angleMultiplicateurEcartX + angleMarge) * 2);
        }
        
    }


    // Update is called once per frame
    void Update()
    {

        // Récupération valeurs caméra
        angleX = mainCam.transform.eulerAngles.x * Mathf.PI / 180;
        angleY = mainCam.transform.eulerAngles.y * Mathf.PI / 180;
        positionX = mainCam.transform.position.x;
        positionY = mainCam.transform.position.y;
        positionZ = mainCam.transform.position.z;

        // Correction valeur angles caméra selon angles objet autour de l'axe nul
        if (angleY > 2 * Mathf.PI - angleMultiplicateurEcartY * angleEcartY && angleObjetY < angleMultiplicateurEcartY * angleEcartY)
        {
            angleY -= 2 * Mathf.PI;
        }
        else if (angleY < angleMultiplicateurEcartY * angleEcartY && angleObjetY > 2 * Mathf.PI - angleMultiplicateurEcartY * angleEcartY)
        {
            angleY += 2 * Mathf.PI;
        }
        if (angleX > 2 * Mathf.PI - angleMultiplicateurEcartX * angleEcartX && angleObjetX < angleMultiplicateurEcartX * angleEcartX)
        {
            angleX -= 2 * Mathf.PI;
        }
        else if (angleX < angleMultiplicateurEcartX * angleEcartX && angleObjetX > 2 * Mathf.PI - angleMultiplicateurEcartX * angleEcartX)
        {
            angleX += 2 * Mathf.PI;
        }

        // Si angleEcartY dépassé
        if (Mathf.Abs(angleY - angleObjetY) > angleEcartY)
        {
            // Actualisation angle Y objet en fonction de l'angle Y caméra
            if (angleY - angleObjetY > 0)
            {
                angleObjetY = angleY - angleEcartY;
            }
            else
            {
                angleObjetY = angleY + angleEcartY;
            }
            // Cas où caméra vers directement le haut ou le bas
            if (angleX > Mathf.PI / 2 - angleMarge && angleX < 3 * Mathf.PI / 2 + angleMarge)
            {
                angleObjetY = angleY;
            }
            // Remise de l'angle Y objet entre 0 et 2*pi
            if (angleObjetY < 0)
            {
                angleObjetY += 2 * Mathf.PI;
            }
            else if (angleObjetY >= 2 * Mathf.PI)
            {
                angleObjetY -= 2 * Mathf.PI;
            }
        }

        // Si angleEcartX dépassé
        if (Mathf.Abs(angleX - angleObjetX) > angleEcartX)
        {
            // Actualisation angle X objet en fonction de l'angle X caméra
            if (angleX - angleObjetX > 0)
            {
                angleObjetX = angleX - angleEcartX;
            }
            else
            {
                angleObjetX = angleX + angleEcartX;
            }
            // Remise de l'angle X objet entre 0 et 2*pi
            if (angleObjetX < 0)
            {
                angleObjetX += 2 * Mathf.PI;
            }
            else if (angleObjetX >= 2 * Mathf.PI)
            {
                angleObjetX -= 2 * Mathf.PI;
            }

        }

        // Définition de la distance sur chaque axe pour obtenir 'distance' entre la caméra et l'objet par coordonnées sphériques
        distanceZ = distance * Mathf.Sin(angleObjetX + Mathf.PI / 2) * Mathf.Cos(angleObjetY);
        distanceX = distance * Mathf.Sin(angleObjetX + Mathf.PI / 2) * Mathf.Sin(angleObjetY);
        distanceY = distance * Mathf.Cos(angleObjetX + Mathf.PI / 2);

        // Actualisation position/rotation de l'objet
        transform.position = new Vector3(positionX + distanceX, positionY + distanceY, positionZ + distanceZ);
        transform.rotation = Quaternion.Euler(angleObjetX * 180 / Mathf.PI, angleObjetY * 180 / Mathf.PI, 0f);
        
    }

}