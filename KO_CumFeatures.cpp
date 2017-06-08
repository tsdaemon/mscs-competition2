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
#include <math.h>

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
    float track_position;
    float disk_number;
    float rank;
    int explicit_lyrics;
    float bpm;
    float gain;
    int country;
    int label;
    
    DefaultRow(string line, bool media, bool test = false) {
        if (!media) {
            vector<int> vect;
            stringstream ss(line);
            
            int i;
            
            while (ss >> i)
            {
                vect.push_back(i);
                
                if (ss.peek() == ',')
                ss.ignore();
            }
            genreId = vect[0];
            timestamp = vect[1];
            mediaId = vect[2];
            albumId = vect[3];
            contextType = vect[4];
            release_date = vect[5];
            platform_name = vect[6];
            platform_family = vect[7];
            media_duration = vect[8];
            listen_type = vect[9];
            user_gender = vect[10];
            user_id = vect[11];
            artist_id = vect[12];
            user_age = vect[13];
            listened = vect[14];
        } else {
            stringstream ss(line);
            
            if (ss.peek() == ',')
            ss.ignore();
            ss>>sampleId;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>timestamp;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>mediaId;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>user_id;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>listened;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>track_position;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>disk_number;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>rank;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>explicit_lyrics;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>bpm;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>gain;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>country;
            if (ss.peek() == ',')
            ss.ignore();
            ss>>label;
            if (ss.peek() == ',')
            ss.ignore();
        }
    }
    
};

template <typename T>
T abs(T val) {
    return val < 0 ? val * -1 : val;
}


template <typename T>
struct Stats {
    Stats() {
        meanDist = 0.0f;
        maxDist = -INT_MAX;
        minDist = INT_MAX;
    }
    float meanDist;
    T maxDist;
    T minDist;
    
    friend ostream& operator<< (ostream& os, const Stats& stats) {
        os<<stats.meanDist<<","<<stats.maxDist<<","<<stats.minDist;
        return os;
    }
};

template <typename T>
vector<float> getCumulativeCountFeatures(vector<DefaultRow*> dataset, T DefaultRow::*field) {
    vector<float> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int totalCount = 0;
        for (int j = i - 1; (j >= 0 && count < 10); j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (prevRow->listened) {
                if (currentRow->user_id != prevRow->user_id) {
                    break;
                }
                if (currentRow->*field == prevRow->*field) {
                    count++;
                }
                totalCount++;
            }
        }
        float val = totalCount > 0 ? float(count) / float(totalCount) : -1.0;
        result.push_back(val);
    }
    return result;
}


template <typename T>
vector<Stats<T>> getCumulativeStatFeatures(vector<DefaultRow*> dataset, T DefaultRow::*field) {
    vector<Stats<T>> result;
    for (int i = 0; i < dataset.size(); i++) {
        Stats<T> temp;
        int count = 0;
        for (int j = i - 1; (j >= 0 && count < 10); j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (prevRow->listened) {
                if (currentRow->user_id != prevRow->user_id) {
                    break;
                }
                T val = abs(prevRow->*field - currentRow->*field);
                temp.maxDist = std::max(temp.maxDist,val);
                temp.minDist = std::min(temp.minDist,val);
                temp.meanDist += val;
                count++;
            }
        }
        if (count > 0) {
            temp.meanDist /= float(count);
        } else {
            temp.meanDist = temp.maxDist = temp.minDist = -1.0;
        }
        result.push_back(temp);
    }
    return result;
}

vector<float> getListenedFeatures(vector<DefaultRow*> dataset) {
    vector<float> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        int listened = 0;
        for (int j = i - 1; (j >= 0 && j >= i - 10); j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (currentRow->user_id != prevRow->user_id) {
                break;
            }
            if (prevRow->listened) listened++;
            count++;
        }
        float val = count > 0 ? float(listened) / float(count) : -1.0;
        result.push_back(val);
    }
    return result;
}

vector<int> getHistoryFeatures(vector<DefaultRow*> dataset) {
    vector<int> result;
    for (int i = 0; i < dataset.size(); i++) {
        int count = 0;
        for (int j = i - 1; j >= 0; j--) {
            DefaultRow *currentRow = dataset[i];
            DefaultRow *prevRow = dataset[j];
            if (currentRow->user_id != prevRow->user_id) {
                break;
            }
            count++;
        }
        result.push_back(count);
    }
    return result;
}


vector<DefaultRow*> getDataset(string filename, bool media, bool test = false) {
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
        if (value != "") dataset.push_back(new DefaultRow(value, media, test));
    }
    return dataset;
}

bool testDataset(vector<DefaultRow*> dataset) {
    for (int i = 1; i < dataset.size(); i++) {
        if ((dataset[i]->user_id == dataset[i - 1]->user_id) && (dataset[i]->timestamp < dataset[i - 1]->timestamp)) {
            return false;
        } else if (dataset[i]->user_id < dataset[i - 1]->user_id) {
            return false;
        }
    }
    return true;
}

int main(int argc, const char * argv[]) {
    // insert code here...
    vector<DefaultRow*> dataset = getDataset("/Users/f/Desktop/ucupnic/data_sort.csv",false);
    
    vector<float>feature = getListenedFeatures(dataset);
    vector<float>feature1 = getCumulativeCountFeatures(dataset, &DefaultRow::genreId);
    vector<float>feature2 = getCumulativeCountFeatures(dataset, &DefaultRow::albumId);
    vector<float>feature3 = getCumulativeCountFeatures(dataset, &DefaultRow::artist_id);
    
    vector<Stats<int>>stat_feature = getCumulativeStatFeatures(dataset, &DefaultRow::media_duration);
    
    dataset = getDataset("/Users/f/Desktop/ucupnic/api_features_concat.csv",true);
    vector<float>feature4 = getCumulativeCountFeatures(dataset, &DefaultRow::explicit_lyrics);
    vector<float>feature5 = getCumulativeCountFeatures(dataset, &DefaultRow::country);
    
    vector<Stats<float>>stat_feature2 = getCumulativeStatFeatures(dataset, &DefaultRow::rank);
    vector<Stats<float>>stat_feature3 = getCumulativeStatFeatures(dataset, &DefaultRow::bpm);
    vector<Stats<float>>stat_feature4 = getCumulativeStatFeatures(dataset, &DefaultRow::gain);
    vector<int>feature6 = getHistoryFeatures(dataset);
    
    ofstream of("/Users/f/Desktop/ucupnic/features.csv");
    
    if (of.is_open()) {
        of<<"percent_listened,genre_listened,album_listened,artist_listened,media_duration_mean,media_duration_max,media_duration_min,explicit_lyrics_percent,country_percent,rank_mean,rank_max,rank_min,bpm_mean,bpm_max,bpm_min,gain_mean,gain_max,gain_min,history"<<endl;
        for (int i = 0; i < feature.size(); i++) {
            of<<feature[i]<<","<<feature1[i]<<","<<feature2[i]<<","<<feature3[i]<<","<<stat_feature[i]<<","<<feature4[i]<<","<<feature5[i]<<","<<stat_feature2[i]<<","<<stat_feature3[i]<<","<<stat_feature4[i]<<","<<feature6[i]<<endl;
        }
        of.close();
    }
    return 0;
}




