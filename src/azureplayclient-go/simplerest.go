package main

import (
	"bytes"
	"encoding/json"
	"errors"
	"fmt"
	"io/ioutil"
	"net/http"
)

type SimpleREST struct {
	baseUri string
	client *http.Client
}

func CreateSimpleRest(baseuri string, client *http.Client) *SimpleREST {
	sr := &SimpleREST{baseUri:baseuri, client:client}
	return sr
}

func (r *SimpleREST) jsonPost(url string, jsonValue []byte) (string, error) {
	u := bytes.NewReader(jsonValue)
	callUri := r.baseUri + url
	req, err := http.NewRequest("POST", callUri, u)
	if err != nil {
		return "", err
	}
	req.Header.Set("Content-Type", "application/json")
	resp, err := r.client.Do(req)
	if err != nil {
		return "", err
	}

	if resp.StatusCode!=200 {
		str := fmt.Sprintf("Failed: %v", resp.Status)
		return "", errors.New(str)
	}

	defer resp.Body.Close()
	result, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return "", err
	}

	return string(result), nil
}

func (r *SimpleREST) jsonRequest(url string) (*bytes.Buffer, error) {
	callUri := r.baseUri + url
	contents, err := r.client.Get(callUri)
	if err != nil {
		return nil, err
	}
	dst := new(bytes.Buffer)

	var body, _ = ioutil.ReadAll(contents.Body)
	json.Indent(dst, body, "", "  ")
	return dst, nil
}