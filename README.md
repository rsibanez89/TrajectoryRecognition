# Trajectory Recognition
This is a visual project made in Unity3D + C#. This project has six scenes:

- `Apply Kmeans And Save Centroids`: This scene reads 3 trajectory from different files, and apply k-means with k clusters. Finally, the centroids are stored in another file.
- `Recognizing Trajectory Using DTW`: This scene trains DTW with 4 different types of trajectories (Circulo, Estiramiento, Nadar, Smash), and then test the recognition accuracy of the technique.
- `Recognizing Trajectory Using StringMatching`: This scene trains StringMatching with 4 different types of trajectories (Circulo, Estiramiento, Nadar, Smash), and then test the recognition accuracy of the technique.
- `Test Entrenamiento3D`: This scene reads a training dataset of a Circle gesture captured with the Microsoft Kinect, and draws all the trajectories corresponding to the right hand.
- `Test Trayectoria3D`: This scene reads a trajectory from a file and draws it. Then, the scene normalize the trajectory and draws it again. Finally, the scene obtains the extreme points of the normalized trajectory (xmin, xmax, ymin, ymax, zmin, zmax) and draws the points.
- `Test UCKmeans`: This scene reads the trajectory training set of the smash, apply k-means with k clusters, and draws the clusters.