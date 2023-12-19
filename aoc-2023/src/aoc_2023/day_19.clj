(ns aoc-2023.day-19
  (:require [aoc-2023.helpers :as helpers]
            [clojure.string :as string]))

(def input-file-path "inputs/day_19/input")
(def sample-file-path "inputs/day_19/sample-1")

(defn parse-wf
  [line]
  (let [[name entry] (string/split line #"\{")
        opentries (string/split (apply str (butlast entry)) #",")
        ops (reduce (fn [out e]
                      (let [[opstr des] (string/split e #":")]
                        (conj out
                              (if (nil? des)
                                {:des opstr}
                                {:op (second opstr)
                                 :value (parse-long
                                         (apply str (drop 2 opstr)))
                                 :target (first opstr)
                                 :des des}))))
                    []
                    opentries)]
    [name ops]))

(defn parse-shape
  [line]
  (reduce (fn [out entry]
            (let [[k v] (string/split entry #"=")]
              (assoc out (first k) (parse-long v))))
          {}
          (string/split (apply str (butlast (rest line))) #",")))

(defn parse-input
  [filename]
  (let [[wflines shapelines] (map #(string/split % #"\n")
                                  (string/split (slurp filename) #"\n\n"))
        workflows (into {} (map parse-wf wflines))
        shapes (map parse-shape shapelines)]
    [workflows shapes]))

(defn apply-wf
  [wfs name shape]
  (some (fn [{:keys [op target value des]}]
          (if op
            (if ((get {\< <} op >) (get shape target) value)
              (if (get wfs des)
                (apply-wf wfs des shape)
                des)
              false)
            (if (get wfs des)
              (apply-wf wfs des shape)
              des)))
        (get wfs name)))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [[wfs shapes] (parse-input filename)]
     (reduce + (map #(if (= "A" (apply-wf wfs "in" %))
                       (reduce + (vals %))
                       0) shapes)))))

(defn get-ranges
  [wfs name curanges]
  (if (= name "A")
    [curanges]
    (if (= name "R")
      []
      (first (reduce
              (fn [[out part] {:keys [op target value des]}]
                (if op
                  (let [[s e] (get part target)]
                    (if (= op \>)
                      (if (and (<= s value) (< value e))
                        [(into out (get-ranges
                                    wfs des
                                    (assoc part target [(inc value) e])))
                         (assoc part target [s value])]
                        [out part])
                      (if (and (< s value) (<= value e))
                        [(into out (get-ranges
                                    wfs des
                                    (assoc part target [s (dec value)])))
                         (assoc part target [value e])]
                        [out part])))
                  (reduced [(into out (get-ranges wfs des part)) []])))
              [[] curanges]
              (get wfs name))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [[wfs _] (parse-input filename)
         ranges (get-ranges wfs "in" {\x [1 4000]
                                      \m [1 4000]
                                      \a [1 4000]
                                      \s [1 4000]})]
     (reduce + (map (fn [entries]
                      (reduce (fn [out [s e]]
                                (* out (- (inc e) s)))
                              1 (vals entries))) ranges)))))

(defn run
  []
  (println (part-one))
  (println (part-two)))