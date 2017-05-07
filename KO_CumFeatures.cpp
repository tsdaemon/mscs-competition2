//
//  main.cpp
//  FeatureExtraction
//
//  Created by Орест on 04.05.17.
//  Copyright © 2017 kupyn. All rights reserved.
//

#include <iostream>
#include <fstream>
#include <vector>
#include <sstream>

using namespace std;

struct DefaultRow {
    long sampleId;
    int genreId;
    long timestamp;
    int mediaId;
    int albumId;
    int contextType;
    long release_date;
    int platform_name;
    int platform_family;
    int media_duration;
    int listen_type;
    int user_gender;
    int user_id;
    int artist_id;
    int user_age;
    int listened;
    
    DefaultRow(string line, bool test) {
        vector<int> vect;
        stringstream ss(line);
        
        int i;
        
        while (ss >> i)
        {
            vect.push_back(i);
            
            if (ss.peek() == ',')
                ss.ignore();
        }
        int offset = test ? 1 : 0;
        sampleId = vect[0 + offset];
        genreId = vect[1 + offset];
        timestamp = vect[2 + offset];
        mediaId = vect[3 + offset];
        albumId = vect[4 + offset];
        contextType = vect[5 + offset];
        release_date = vect[6 + offset];
        platform_name = vect[7 + offset];
        platform_family = vect[8 + offset];
        media_duration = vect[9 + offset];
        listen_type = vect[10 + offset];
        user_gender = vect[11 + offset];
        user_id = vect[12 + offset];
        artist_id = vect[13 + offset];
        user_age = vect[14 + offset];
        listened = test ? -1 : vect[15];
    }
    
};

vector<float> getGenreFeatures(vector<DefaultRow*> dataset) {
    vector<float> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int listened = 0;
        for (int j = i - 1; j >= 0; j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (currentRow->user_id != prevRow->user_id) {
                break;
            }
            if (currentRow->genreId == prevRow->genreId) {
                if (prevRow->listened) listened++;
                count++;
            }
        }
        float val = count > 0 ? float(listened) / float(count) : -1.0;
        result.push_back(val);
    }
    return result;
}

vector<float> getGenreTestFeatures(vector<DefaultRow*> dataset, vector<DefaultRow*> testDataset) {
    vector<float> result;
    int user = 0;
    int count = 0;
    int listened = 0;
    for (int i = 0; i < dataset.size(); i++) {
        if (dataset[i]->user_id == testDataset[user]->user_id) {
            if (dataset[i]->genreId == testDataset[user]->genreId) {
                if (dataset[i]->listened) listened++;
                count++;
            }
        } else {
            user++;
            float val = count > 0 ? float(listened) / float(count) : -1.0;
            result.push_back(val);
            count = 0;
            listened = 0;
            i--;
        }
    }
    return result;
}

vector<float> getArtistFeatures(vector<DefaultRow*> dataset) {
    vector<float> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int listened = 0;
        for (int j = i - 1; j >= 0; j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (currentRow->user_id != prevRow->user_id) {
                break;
            }
            if (currentRow->artist_id == prevRow->artist_id) {
                if (prevRow->listened) listened++;
                count++;
            }
        }
        float val = count > 0 ? float(listened) / float(count) : -1.0;
        result.push_back(val);
    }
    return result;
}

vector<float> getArtistTestFeatures(vector<DefaultRow*> dataset, vector<DefaultRow*> testDataset) {
    vector<float> result;
    int user = 0;
    int count = 0;
    int listened = 0;
    for (int i = 0; i < dataset.size(); i++) {
        if (dataset[i]->user_id == testDataset[user]->user_id) {
            if (dataset[i]->artist_id == testDataset[user]->artist_id) {
                if (dataset[i]->listened) listened++;
                count++;
            }
        } else {
            user++;
            float val = count > 0 ? float(listened) / float(count) : -1.0;
            result.push_back(val);
            count = 0;
            listened = 0;
            i--;
        }
    }
    return result;
}

vector<float> getAlbumFeatures(vector<DefaultRow*> dataset) {
    vector<float> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int listened = 0;
        for (int j = i - 1; j >= 0; j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (currentRow->user_id != prevRow->user_id) {
                break;
            }
            if (currentRow->albumId == prevRow->albumId) {
                if (prevRow->listened) listened++;
                count++;
            }
        }
        float val = count > 0 ? float(listened) / float(count) : -1.0;
        result.push_back(val);
    }
    return result;
}

vector<float> getAlbumTestFeatures(vector<DefaultRow*> dataset, vector<DefaultRow*> testDataset) {
    vector<float> result;
    int user = 0;
    int count = 0;
    int listened = 0;
    for (int i = 0; i < dataset.size(); i++) {
        if (dataset[i]->user_id == testDataset[user]->user_id) {
            if (dataset[i]->albumId == testDataset[user]->albumId) {
                if (dataset[i]->listened) listened++;
                count++;
            }
        } else {
            user++;
            float val = count > 0 ? float(listened) / float(count) : -1.0;
            result.push_back(val);
            count = 0;
            listened = 0;
            i--;
        }
    }
    return result;
}

vector<int> getTolikFeatures(vector<DefaultRow*> dataset) {
    vector<int> result;
    int counter = 0;
    result.push_back(counter);
    for (int i = 1; i < dataset.size(); i++) {
        DefaultRow *currentRow = dataset[i];
        if (currentRow->user_id != dataset[i - 1]->user_id) {
            counter = 0;
        } else {
            if (dataset[i - 1]->listened) counter ++;
            else counter = 0;
        }
        result.push_back(counter);
    }
    return result;
}

vector<int> getTolikTestFeatures(vector<DefaultRow*> dataset, vector<DefaultRow*> testDataset) {
    vector<int> result;
    int user = 0;
    int count = 0;
    for (int i = 1; i < dataset.size(); i++) {
        if (dataset[i - 1]->listened) {
            count++;
        } else count = 0;
        if (dataset[i]->user_id != testDataset[user]->user_id) {
            user++;
            result.push_back(count);
        }
    }
    return result;
}

vector<float> getSongFeatures(vector<DefaultRow*> dataset) {
    vector<float> result;
    vector<int> count_v;
    vector<int> listened_v;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int listened = 0;
         for (int j = i - 1; j >= 0; j--) {
             if (dataset[i]->mediaId == dataset[j]->mediaId) {
                 count = count_v[j] + 1;
                listened = dataset[j]->listened ?  listened_v[j] + 1 : listened_v[j];
                break;
             }
        }
        count_v.push_back(count);
        listened_v.push_back(listened);
    }
    
    for (int i = 0; i < dataset.size(); i++) {
        float val = count_v[i] > 0 ? float(listened_v[i]) / float(count_v[i]) : -1.0;
        result.push_back(val);
    }
    
    return result;
}

vector<float> getSongTestFeatures(vector<DefaultRow*> dataset, vector<DefaultRow*> testDataset) {
    vector<float> result;
    for (int i = 0; i < testDataset.size(); i++) {
        int count = 0;
        int listened = 0;
        for (int j = 0; j < dataset.size(); j++) {
            if (dataset[j]->mediaId == testDataset[i]->mediaId) {
                count++;
                if (dataset[j]->listened) {
                    listened++;
                }
            }
        }
        float val = count > 0 ? float(listened) / float(count) : -1.0;
        result.push_back(val);
    }
    return result;
}


vector<DefaultRow*> getDataset(string filename, bool test) {
    ifstream file ( filename );
    string value;
    vector<DefaultRow*> dataset;
    bool first = true;
    while ( file.good() )
    {
        getline ( file, value, '\n' );
        if (first) {
            first = false;
            continue;
        }
        if (value != "") dataset.push_back(new DefaultRow(value, test));
    }
    return dataset;
}

int main(int argc, const char * argv[]) {
    // insert code here...
    vector<DefaultRow*> datasetTest = getDataset("/Users/f/Desktop/ucupnic/test_sort.csv",true);
    vector<DefaultRow*> dataset = getDataset("/Users/f/Desktop/ucupnic/train_sort.csv",false);
    
//    vector<float>genres = getSongFeatures(dataset);
//    ofstream output_file("/Users/f/Desktop/ucupnic/song_listened.txt");
//    ostream_iterator<float> output_iterator(output_file, "\n");
//    copy(genres.begin(), genres.end(), output_iterator);
    
    vector<float>genres = getSongTestFeatures(dataset, datasetTest);
    ofstream output_file1("/Users/f/Desktop/ucupnic/song_listened_test.txt");
    ostream_iterator<float> output_iterator1(output_file1, "\n");
    copy(genres.begin(), genres.end(), output_iterator1);
    
//    vector<float>genres = getArtistFeatures(dataset);
//    ofstream output_file("/Users/f/Desktop/ucupnic/artist_listened.txt");
//    ostream_iterator<float> output_iterator(output_file, "\n");
//    copy(genres.begin(), genres.end(), output_iterator);
//    
//    genres = getArtistTestFeatures(dataset, datasetTest);
//    ofstream output_file1("/Users/f/Desktop/ucupnic/artist_test_listened.txt");
//    ostream_iterator<float> output_iterator1(output_file1, "\n");
//    copy(genres.begin(), genres.end(), output_iterator1);
//    
//    vector<float>albums = getAlbumFeatures(dataset);
//    ofstream output_file2("/Users/f/Desktop/ucupnic/album_listened.txt");
//    ostream_iterator<float> output_iterator2(output_file2, "\n");
//    copy(albums.begin(), albums.end(), output_iterator2);
//    
//    albums = getAlbumTestFeatures(dataset, datasetTest);
//    ofstream output_file3("/Users/f/Desktop/ucupnic/albums_test_listened.txt");
//    ostream_iterator<float> output_iterator3(output_file3, "\n");
//    copy(albums.begin(), albums.end(), output_iterator3);

    
    return 0;
}
